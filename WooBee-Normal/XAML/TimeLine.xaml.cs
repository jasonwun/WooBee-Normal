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


        //private ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
        
        //ScrollViewer _scrollViewer;
        //Compositor _compositor;
        //CompositionPropertySet _scrollerViewerManipulation;
        //ExpressionAnimation _rotationAnimation, _opacityAnimation, _offsetAnimation;
        //ScalarKeyFrameAnimation _resetAnimation, _loadingAnimation;

        //Visual _borderVisual;
        //Visual _refreshIconVisual;
        //float _refreshIconOffsetY;
        //const float REFRESH_ICON_MAX_OFFSET_Y = 36.0f;

        //bool _refresh;
        //DateTime _pulledDownTime, _restoredTime;

        public TimeLine()
        {
            this.InitializeComponent();
            listView.ItemsSource = incrementalSource;
            ShowStatusBar();
            //DownloadEmotion();
        }

        private async void DownloadEmotion()
        {
            string Uri = "https://api.weibo.com/2/emotions.json?source=";
            Uri += App.client_id;
            Uri += "&access_token=";
            Uri += App.access_token;
            Uri += "&type=ani";

            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            sinceid++;
            IncrementalSource NewIncrementalSource = new IncrementalSource(sinceid);
            listView.ItemsSource = NewIncrementalSource;
        }


        private void SentWeiboButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SendWeibo));
        }

        private void GotocommentButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Message));
        }

        private void listView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var _listview = listView as ListView;
            _weibo = (Weibo)_listview.SelectedItem;
            Frame.Navigate(typeof(WeiboDetail), _weibo);
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PhotoPage));
        }


        private async void ShowStatusBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = StatusBar.GetForCurrentView();
                await statusbar.HideAsync();
            }
        }

    }
}
