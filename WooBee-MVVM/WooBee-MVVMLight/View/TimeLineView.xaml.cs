using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using WooBee_MVVM.Model;
using WooBee_MVVMLight.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_MVVMLight.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimeLineView: BindablePage
    {

        public TimeLineViewModel TLVm { get; set; }
        ScrollViewer _scrollViewer;
        CompositionPropertySet _scrollerViewerManipulation;
        Compositor _compositor;
        ExpressionAnimation _rotationAnimation;
        ExpressionAnimation _opacityAnimation;
        ExpressionAnimation _offsetAnimation;
        ScalarKeyFrameAnimation _resetAnimation;
        ScalarKeyFrameAnimation _loadingAnimation;
        Visual _refreshIconVisual;
        Visual _borderVisual;
        const float REFRESH_ICON_MAX_OFFSET_Y = 36.0f;
        bool _refresh;
        DateTime _pulledDownTime, _restoredTime;
        float _refreshIconOffsetY;

        public TimeLineView()
        {
            this.InitializeComponent();
            this.DataContext = TLVm = new TimeLineViewModel();
            DisableStatusBar();
        }

        private async void DisableStatusBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = StatusBar.GetForCurrentView();
                await statusbar.HideAsync();
            }
        }

        private void listView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var _listview = listView as ListView;
            var _weibo = (Weibo)_listview.SelectedItem;
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
                        //Frame.Navigate(typeof(HashTagPage), hashtag);
                    }
                    else if (advancesource.Text.Substring(0, 1) == "@")
                    {
                        string username = advancesource.Text.Substring(1);
                        NavigationParameter navip = new NavigationParameter(username, "screen_name");
                        Frame.Navigate(typeof(UserView), navip);
                    }
                    else if (_weibo.User.ScreenName != null && advancesource.Text == _weibo.User.ScreenName)
                    {
                        string username = advancesource.Text;
                        NavigationParameter navip = new NavigationParameter(username, "screen_name");
                        Frame.Navigate(typeof(UserView), navip);
                    }
                    else if(advancesource.Text == _weibo.RepostWeibo.User.ScreenName)
                    {
                        string username = advancesource.Text;
                        NavigationParameter navip = new NavigationParameter(username, "screen_name");
                        Frame.Navigate(typeof(UserView), navip);
                    }
                    else
                    {
                        Frame.Navigate(typeof(WeiboDetailView), _weibo);
                    }
                }

                else if (imageSource != null)
                {
                    string username = _weibo.User.ScreenName;
                    NavigationParameter navip = new NavigationParameter(username, "screen_name");
                    Frame.Navigate(typeof(UserView), navip);
                }
                else if (richtextBlock != null)
                {
                    Frame.Navigate(typeof(WeiboDetailView), _weibo);
                }
            }

            else
            {
                Frame.Navigate(typeof(WeiboDetailView), _weibo);
            }
        }

        private void PrepareExpressionAnimationsOnScroll()
        {
            _refreshIconVisual.StartAnimation("RotationAngleInDegrees", _rotationAnimation);
            _refreshIconVisual.StartAnimation("Opacity", _opacityAnimation);
            _refreshIconVisual.StartAnimation("Offset.Y", _offsetAnimation);
            _borderVisual.StartAnimation("Offset.Y", _offsetAnimation);
        }

        void OnDirectManipulationStarted(object sender, object e)
        {
            // QUESTION 1
            // I cannot think of a better way to monitor overpan changes, maybe there should be an Animating event?
            //
            Windows.UI.Xaml.Media.CompositionTarget.Rendering += OnCompositionTargetRendering;

            // Initialise the values.
            _refresh = false;
        }

        async void OnDirectManipulationCompleted(object sender, object e)
        {
            Windows.UI.Xaml.Media.CompositionTarget.Rendering -= OnCompositionTargetRendering;

            //Debug.WriteLine($"ScrollViewer Rollback animation duration: {(_restoredTime - _pulledDownTime).Milliseconds}");

            // The ScrollViewer's rollback animation is appx. 200ms. So if the duration between the two DateTimes we recorded earlier
            // is greater than 250ms, we should cancel the refresh.
            var cancelled = (_restoredTime - _pulledDownTime) > TimeSpan.FromMilliseconds(250);

            if (_refresh)
            {
                if (cancelled)
                {
                    Debug.WriteLine("Refresh cancelled...");

                    StartResetAnimations();
                }
                else
                {
                    Debug.WriteLine("Refresh now!!!");

                    await StartLoadingAnimation(() => StartResetAnimations());
                }
            }
        }

        private void Root_Unloaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer.DirectManipulationStarted -= OnDirectManipulationStarted;
            _scrollViewer.DirectManipulationCompleted -= OnDirectManipulationCompleted;
        }

        private void Root_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = listView.GetScrollViewer();
            _scrollViewer.DirectManipulationStarted += OnDirectManipulationStarted;
            _scrollViewer.DirectManipulationCompleted += OnDirectManipulationCompleted;

            // Retrieve the ScrollViewer manipulation and the Compositor.
            _scrollerViewerManipulation = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(_scrollViewer);
            _compositor = _scrollerViewerManipulation.Compositor;

            // At the moment there are three things happening when pulling down the list -
            // 1. The Refresh Icon fades in.
            // 2. The Refresh Icon rotates (400°).
            // 3. The Refresh Icon gets pulled down a bit (REFRESH_ICON_MAX_OFFSET_Y)
            // QUESTION 5
            // Can we also have Geometric Path animation so we can also draw the Refresh Icon along the way?
            //

            // Create a rotation expression animation based on the overpan distance of the ScrollViewer.
            _rotationAnimation = _compositor.CreateExpressionAnimation("min(max(0, ScrollManipulation.Translation.Y) * Multiplier, MaxDegree)");
            _rotationAnimation.SetScalarParameter("Multiplier", 10.0f);
            _rotationAnimation.SetScalarParameter("MaxDegree", 400.0f);
            _rotationAnimation.SetReferenceParameter("ScrollManipulation", _scrollerViewerManipulation);

            // Create an opacity expression animation based on the overpan distance of the ScrollViewer.
            _opacityAnimation = _compositor.CreateExpressionAnimation("min(max(0, ScrollManipulation.Translation.Y) / Divider, 1)");
            _opacityAnimation.SetScalarParameter("Divider", 30.0f);
            _opacityAnimation.SetReferenceParameter("ScrollManipulation", _scrollerViewerManipulation);

            // Create an offset expression animation based on the overpan distance of the ScrollViewer.
            _offsetAnimation = _compositor.CreateExpressionAnimation("(min(max(0, ScrollManipulation.Translation.Y) / Divider, 1)) * MaxOffsetY");
            _offsetAnimation.SetScalarParameter("Divider", 30.0f);
            _offsetAnimation.SetScalarParameter("MaxOffsetY", REFRESH_ICON_MAX_OFFSET_Y);
            _offsetAnimation.SetReferenceParameter("ScrollManipulation", _scrollerViewerManipulation);

            // Create a keyframe animation to reset properties like Offset.Y, Opacity, etc.
            _resetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _resetAnimation.InsertKeyFrame(1.0f, 0.0f);

            // Create a loading keyframe animation (in this case, a rotation animation). 
            _loadingAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _loadingAnimation.InsertKeyFrame(1.0f, 360);
            _loadingAnimation.Duration = TimeSpan.FromMilliseconds(1200);
            _loadingAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            // Get the RefreshIcon's Visual.
            _refreshIconVisual = ElementCompositionPreview.GetElementVisual(RefreshIcon);
            // Set the center point for the rotation animation.
            _refreshIconVisual.CenterPoint = new Vector3(Convert.ToSingle(RefreshIcon.ActualWidth / 2), Convert.ToSingle(RefreshIcon.ActualHeight / 2), 0);

            // Get the ListView's inner Border's Visual.
            var border = (Border)VisualTreeHelper.GetChild(listView, 0);
            _borderVisual = ElementCompositionPreview.GetElementVisual(border);

            PrepareExpressionAnimationsOnScroll();
        }

        async Task StartLoadingAnimation(Action completed)
        {
            // Create a short delay to allow the expression rotation animation to more smoothly transition
            // to the new keyframe animation
            await Task.Delay(100);

            _refreshIconVisual.StartAnimation("RotationAngleInDegrees", _loadingAnimation);

            await FakeServiceCall();
            completed();
        }

        void StartResetAnimations()
        {
            var batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            // Looks like expression aniamtions will be removed after the following keyframe
            // animations have run. So here I have to re-start them once the keyframe animations
            // are completed.
            batch.Completed += (s, e) => PrepareExpressionAnimationsOnScroll();

            _borderVisual.StartAnimation("Offset.Y", _resetAnimation);
            _refreshIconVisual.StartAnimation("Opacity", _resetAnimation);
            batch.End();
        }

        async Task FakeServiceCall()
        {
            await Task.Delay(2000);

            TLVm.Refresh();
        }

        private async void listView_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = listView.GetScrollViewer();
            TimeSpan period = TimeSpan.FromMilliseconds(35);
            Windows.System.Threading.ThreadPoolTimer.CreateTimer(async (source) => {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
                {
                    _scrollViewer.ChangeView(null, App._scrollViewerVerticalOffset, null,true);
                }));
            }, period);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App._scrollViewerVerticalOffset = _scrollViewer.VerticalOffset;
            base.OnNavigatedFrom(e);
        }

        private async void FloatTab_Holding(object sender, HoldingRoutedEventArgs e)
        {
            _scrollViewer = listView.GetScrollViewer();
            bool succeed = _scrollViewer.ChangeView(null, 0, null);
            await Task.Yield();
        }

        void OnCompositionTargetRendering(object sender, object e)
        {
            // QUESTION 2
            // What I've noticed is that I have to manually stop and
            // start the animation otherwise the Offset.Y is 0. Why?
            //
            _refreshIconVisual.StopAnimation("Offset.Y");

            // QUESTION 3
            // Why is the Translation always (0,0,0)?
            //
            //Vector3 translation;
            //var translationStatus = _scrollerViewerManipulation.TryGetVector3("Translation", out translation);
            //switch (translationStatus)
            //{
            //    case CompositionGetValueStatus.Succeeded:
            //        Debug.WriteLine($"ScrollViewer's Translation Y: {translation.Y}");
            //        break;
            //    case CompositionGetValueStatus.TypeMismatch:
            //    case CompositionGetValueStatus.NotFound:
            //    default:
            //        break;
            //}

            _refreshIconOffsetY = _refreshIconVisual.Offset.Y;
            //Debug.WriteLine($"RefreshIcon's Offset Y: {_refreshIconOffsetY}");

            // Question 4
            // It's not always the case here as the user can pull it all the way down and then push it back up to
            // CANCEL a refresh!! Though I cannot seem to find an easy way to detect right after the finger is lifted.
            // DirectManipulationCompleted is called too late.
            // What might be really helpful is to have a DirectManipulationDelta event with velocity and other values.
            //
            // At the moment I am calculating the time difference between the list gets pulled all the way down and rolled back up.
            // 
            if (!_refresh)
            {
                _refresh = _refreshIconOffsetY == REFRESH_ICON_MAX_OFFSET_Y;
            }

            if (_refreshIconOffsetY == REFRESH_ICON_MAX_OFFSET_Y)
            {
                _pulledDownTime = DateTime.Now;
                //Debug.WriteLine($"When the list is pulled down: {_pulledDownTime}");

                // Stop the Opacity animation on the RefreshIcon and the Offset.Y animation on the Border (ScrollViewer's host)
                _refreshIconVisual.StopAnimation("Opacity");
                _borderVisual.StopAnimation("Offset.Y");
            }

            if (_refresh && _refreshIconOffsetY <= 1)
            {
                _restoredTime = DateTime.Now;
                //Debug.WriteLine($"When the list is back up: {_restoredTime}");
            }

            _refreshIconVisual.StartAnimation("Offset.Y", _offsetAnimation);
        }

        private void image1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MultiImgNavigationParam para = new MultiImgNavigationParam();
            var item = (Weibo)(sender as Image).DataContext;
            if (item.RepostWeibo != null)
            {
                para._imgSource = item.RepostWeibo.PicUrls;
                Frame.Navigate(typeof(PhotoPage), para);
            }

            else
            {
                para._imgSource = item.PicUrls;
                Frame.Navigate(typeof(PhotoPage), para);
            }
        }

        private void gridView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MultiImgNavigationParam para = new MultiImgNavigationParam();
            var item = (Weibo)(sender as GridView).DataContext;
            if (item.RepostWeibo != null)
            {
                para._imgSource = item.RepostWeibo.PicUrls;
                para.ImgIndex = (sender as GridView).SelectedIndex;
                Frame.Navigate(typeof(PhotoPage), para);
            }

            else
            {
                para._imgSource = item.PicUrls;
                para.ImgIndex = (sender as GridView).SelectedIndex;
                Frame.Navigate(typeof(PhotoPage), para);
            }

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await Task.Yield();
            await TLVm.RefreshNotification();
            App.IsRefresh = false;
            
            base.OnNavigatedTo(e);
        }
    }
}
