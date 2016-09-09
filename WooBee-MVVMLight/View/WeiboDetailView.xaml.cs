using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WooBee_MVVM.Model;
using WooBee_MVVMLight.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_MVVMLight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeiboDetailView : BindablePage
    {
        public WeiboDetailViewModel WeiboDV { get; set; }
        private Weibo weibo { get; set; }
        public WeiboDetailView()
        {
            this.InitializeComponent();
            DataContext = WeiboDV = new WeiboDetailViewModel();
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


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            Weibo _weibo = e.Parameter as Weibo;
            WeiboDV.Weibo = _weibo;
            weibo = _weibo;
            base.OnNavigatedTo(e);
        }

        private void replyButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Comment comment = (Comment)(sender as Button).DataContext;
            PostWeiboNaviParamContainer postcontainer = new PostWeiboNaviParamContainer();
            postcontainer.PostType = "replycomment";
            postcontainer.WeiboID = comment.Status.ID;
            postcontainer.CommentID = comment.ID;
            NavigationService.NaivgateToPage(typeof(NewPostView), postcontainer);
        }

        private void ReShareButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Weibo weibo = (Weibo)(sender as Button).DataContext;
            PostWeiboNaviParamContainer postcontainer = new PostWeiboNaviParamContainer();
            postcontainer.PostType = "replyrepost";
            postcontainer.WeiboID = weibo.ID;
            NavigationService.NaivgateToPage(typeof(NewPostView), postcontainer);
        }
    }
}
