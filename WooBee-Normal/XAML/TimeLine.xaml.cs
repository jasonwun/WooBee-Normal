using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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


        public TimeLine()
        {
            this.InitializeComponent();
            listView.ItemsSource = incrementalSource;
            ShowStatusBar();
            GetNewMessage();
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
            var item = (Weibo)(sender as Image).DataContext;
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
