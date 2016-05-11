using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_Normal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimeLine : Page
    {
        public Weibo _weibo { get; set; }
        private HomeWeibo _homeweibo { get; set; }
        IncrementalSource incrementalSource = new IncrementalSource(sinceid);
        private static int sinceid = 0;

        CompositionPropertySet _scrollerViewerManipulation;
        ScrollViewer _scrollViewer;
        Compositor _compositor;
        ExpressionAnimation _rotationAnimation, _opacityAnimation, _offsetAnimation;
        ScalarKeyFrameAnimation _resetAnimation, _loadingAnimation;

        Visual _borderVisual;
        Visual _refreshIconVisual;
        float _refreshIconOffsetY;
        const float REFRESH_ICON_MAX_OFFSET_Y = 36.0f;

        bool _refresh;
        DateTime _pulledDownTime, _restoredTime;
        LinearEasingFunction _linear;


        public TimeLine()
        {
            this.InitializeComponent();
            listView.ItemsSource = incrementalSource;
            ShowStatusBar();
            GetNewMessage();

            this.Loaded += (s, e) =>
            {
                _scrollViewer = listView.GetScrollViewer();
                _scrollViewer.DirectManipulationStarted += OnDirectManipulationStarted;
                _scrollViewer.DirectManipulationCompleted += OnDirectManipulationCompleted;

                _scrollerViewerManipulation = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(_scrollViewer);
                _compositor = _scrollerViewerManipulation.Compositor;

                _rotationAnimation = _compositor.CreateExpressionAnimation("min(max(0, ScrollManipulation.Translation.Y) * Multiplier, MaxDegree)");
                _rotationAnimation.SetScalarParameter("Multiplier", 10.0f);
                _rotationAnimation.SetScalarParameter("MaxDegree", 360.0f);
                _rotationAnimation.SetReferenceParameter("ScrollManipulation", _scrollerViewerManipulation);

                _opacityAnimation = _compositor.CreateExpressionAnimation("min(max(0, ScrollManipulation.Translation.Y) / Divider, 1)");
                _opacityAnimation.SetScalarParameter("Divider", 30.0f);
                _opacityAnimation.SetReferenceParameter("ScrollManipulation", _scrollerViewerManipulation);

                _offsetAnimation = _compositor.CreateExpressionAnimation("(min(max(0, ScrollManipulation.Translation.Y) / Divider, 1)) * MaxOffsetY");
                _offsetAnimation.SetScalarParameter("Divider", 30.0f);
                _offsetAnimation.SetScalarParameter("MaxOffsetY", REFRESH_ICON_MAX_OFFSET_Y);
                _offsetAnimation.SetReferenceParameter("ScrollManipulation", _scrollerViewerManipulation);

                _resetAnimation = _compositor.CreateScalarKeyFrameAnimation();
                _resetAnimation.InsertKeyFrame(1.0f, 0.0f);


                _loadingAnimation = _compositor.CreateScalarKeyFrameAnimation();
                _loadingAnimation.InsertKeyFrame(0.00f, 0.00f, _linear);
                _loadingAnimation.InsertKeyFrame(0.20f, 72.0f, _linear);
                _loadingAnimation.InsertKeyFrame(0.40f, 144.00f, _linear);
                _loadingAnimation.InsertKeyFrame(0.60f, 216.0f, _linear);
                _loadingAnimation.InsertKeyFrame(0.80f, 288.00f, _linear);
                _loadingAnimation.InsertKeyFrame(1.00f, 360.0f, _linear);
                _loadingAnimation.Duration = TimeSpan.FromMilliseconds(250);
                _loadingAnimation.IterationBehavior = AnimationIterationBehavior.Forever;


                _refreshIconVisual = ElementCompositionPreview.GetElementVisual(RefreshIcon);

                _refreshIconVisual.CenterPoint = new Vector3(Convert.ToSingle(RefreshIcon.ActualWidth / 2), Convert.ToSingle(RefreshIcon.ActualHeight / 2), 0);


                var border = (Border)VisualTreeHelper.GetChild(listView, 0);
                _borderVisual = ElementCompositionPreview.GetElementVisual(border);

                PrepareExpressionAnimationsOnScroll();
            };

            this.Unloaded += (s, e) =>
            {
                _scrollViewer.DirectManipulationStarted -= OnDirectManipulationStarted;
                _scrollViewer.DirectManipulationCompleted -= OnDirectManipulationCompleted;
            };
        }

        private void PrepareExpressionAnimationsOnScroll()
        {
            _refreshIconVisual.StartAnimation("RotationAngleinDegrees", _rotationAnimation);
            _refreshIconVisual.StartAnimation("Opacity", _opacityAnimation);
            _refreshIconVisual.StartAnimation("Offset.Y", _offsetAnimation);
            _borderVisual.StartAnimation("Offset.Y", _offsetAnimation);
        }

        private async void OnDirectManipulationCompleted(object sender, object e)
        {
            Windows.UI.Xaml.Media.CompositionTarget.Rendering -= OnCompositionTargetRendering;

            var cancelled = (_restoredTime - _pulledDownTime) > TimeSpan.FromMilliseconds(250);

            if (_refresh)
            {
                if (cancelled)
                {
                    StartResetAnimations();
                }
                else
                {
                    await StartLoadingAnimation(() => StartResetAnimations());
                }
            }
        }

        void StartResetAnimations()
        {
            var batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            
            batch.Completed += (s, e) => PrepareExpressionAnimationsOnScroll();

            _borderVisual.StartAnimation("Offset.Y", _resetAnimation);
            _refreshIconVisual.StartAnimation("Opacity", _resetAnimation);
            batch.End();
        }

        async Task StartLoadingAnimation(Action completed)
        {
            await Task.Delay(100);

            _refreshIconVisual.StartAnimation("RotationAngleInDegrees", _loadingAnimation);

            RefreshSource();

            completed();
        }

        private void RefreshSource()
        {
            sinceid++;
            IncrementalSource NewIncrementalSource = new IncrementalSource(sinceid);
            listView.ItemsSource = NewIncrementalSource;
            GetNewMessage();
        }

        private void OnCompositionTargetRendering(object sender, object e)
        {
            _refreshIconVisual.StopAnimation("Offset.Y");

            _refreshIconOffsetY = _refreshIconVisual.Offset.Y;

            if (!_refresh)
            {
                _refresh = _refreshIconOffsetY == REFRESH_ICON_MAX_OFFSET_Y;
            }

            if (_refreshIconOffsetY == REFRESH_ICON_MAX_OFFSET_Y)
            {
                _pulledDownTime = DateTime.Now;
                
                _refreshIconVisual.StopAnimation("Opacity");
                _borderVisual.StopAnimation("Offset.Y");
            }

            if (_refresh && _refreshIconOffsetY <= 1)
            {
                _restoredTime = DateTime.Now;
                
            }

            _refreshIconVisual.StartAnimation("Offset.Y", _offsetAnimation);
        }

        private void OnDirectManipulationStarted(object sender, object e)
        {
            Windows.UI.Xaml.Media.CompositionTarget.Rendering += OnCompositionTargetRendering;

            
            _refresh = false;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            sinceid++;
            IncrementalSource NewIncrementalSource = new IncrementalSource(sinceid);
            listView.ItemsSource = NewIncrementalSource;
            GetNewMessage();
        }

        private async void GetNewMessage()
        {
            string Uri = "https://rm.api.weibo.com/2/remind/unread_count.json?";
            Uri += "&access_token=";
            Uri += App.weico_access_token;

            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            RemindMessageModel _remindMessageModel = new RemindMessageModel();
            _remindMessageModel = JsonConvert.DeserializeObject<RemindMessageModel>(strResponse);
            int total = 0;
            
            if (_remindMessageModel.Notice != 0 || _remindMessageModel.Mention_status != 0 || _remindMessageModel.Cmt != 0 || _remindMessageModel.Follower != 0)
            {
                PopUpRoot.Visibility = Visibility.Visible;
                total += _remindMessageModel.Cmt;
                total += _remindMessageModel.Follower;
                total += _remindMessageModel.Mention_status;
                MessagePopUp.Text = total.ToString();
                
            }
                
        }

        private void SentWeiboButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SendWeibo));
        }

        private void GotocommentButton_Click(object sender, RoutedEventArgs e)
        {
            ResetMessage();
            Frame.Navigate(typeof(Message));
        }

        private async void ResetMessage()
        {
            string Uri = "https://rm.api.weibo.com/2/remind/set_count.json?";
            Uri += "&access_token=";
            Uri += App.weico_access_token;
            Uri += "&type=follower,cmt,dm,mention_status,mention_cmt";

            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
        }

        private void listView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var _listview = listView as ListView;
            _weibo = (Weibo)_listview.SelectedItem;
            ListViewItemPresenter originalsource = e.OriginalSource as ListViewItemPresenter;
            if (originalsource == null)
            {
                var advancesource = e.OriginalSource as TextBlock;
                var imageSource = e.OriginalSource as Image;
                var richtextBlock = e.OriginalSource as RichTextBlock;
                if (advancesource != null)
                {
                    if (advancesource.Text == "网页链接")
                    {
                        e.Handled = true;
                    }
                    else if (advancesource.Text.Substring(0, 1) == "#")
                    {
                        string hashtag = advancesource.Text.Substring(1);
                        hashtag = hashtag.Remove(hashtag.Length - 1);
                        Frame.Navigate(typeof(HashTagPage), hashtag);
                    }
                    else if (advancesource.Text.Substring(0, 1) == "@")
                    {
                        string username = advancesource.Text.Substring(1);
                        Frame.Navigate(typeof(UserPage), username);
                    }
                    else if (_weibo.User.ScreenName != null && advancesource.Text == _weibo.User.ScreenName)
                    {
                        string username = advancesource.Text;
                        Frame.Navigate(typeof(UserPage), username);
                    }
                    else
                    {
                        Frame.Navigate(typeof(WeiboDetail), _weibo);
                    }
                }
                
                else if(imageSource != null)
                {
                    string username = _weibo.User.ScreenName;
                    Frame.Navigate(typeof(UserPage), username);
                }
                else if(richtextBlock != null)
                {
                    Frame.Navigate(typeof(WeiboDetail), _weibo);
                }
                
 
            }
            
            else
            {
                Frame.Navigate(typeof(WeiboDetail), _weibo);
            }

            
        }

        private async void ShowStatusBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = StatusBar.GetForCurrentView();
                await statusbar.HideAsync();
            }
        }

        private void image1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PageParametersContainers para = new PageParametersContainers();
            var item = (Weibo)(sender as PlaceHolderImage).DataContext;
            if (item.RepostWeibo != null)
            {
                para.parameter1 = item.RepostWeibo.PicUrls;
                Frame.Navigate(typeof(PhotoPage), para);
            }

            else
            {
                para.parameter1 = item.PicUrls;
                Frame.Navigate(typeof(PhotoPage), para);
            }
        }

        private void gridView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PageParametersContainers para = new PageParametersContainers();
            var item = (Weibo)(sender as GridView).DataContext;
            if (item.RepostWeibo != null)
            {
                para.parameter1 = item.RepostWeibo.PicUrls;
                para.parameter2 = (sender as GridView).SelectedIndex;
                Frame.Navigate(typeof(PhotoPage), para);
            }
                
            else
            {
                para.parameter1 = item.PicUrls;
                para.parameter2 = (sender as GridView).SelectedIndex;
                Frame.Navigate(typeof(PhotoPage), para);
            }

        }

        private void HashTagHyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HashTagPage));
        }

    }
}
