using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_Normal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeiboDetail : Page
    {
        public Weibo _weibo { get; set; }
        ObservableCollection<Weibo> _weiboSource = new ObservableCollection<Weibo>();
        CommentUti commentuti = new CommentUti();
        RepostUti repostUti = new RepostUti();
        public WeiboDetail()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _weibo = e.Parameter as Weibo;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ScreenNameTextBlock.Text = _weibo.User.ScreenName;
            var uri = new Uri(_weibo.User.AvatarLarge);
            var bitmap = new BitmapImage(uri);
            WeiboUserAvatar.Source = bitmap;
            _weiboSource.Add(_weibo);
            CommentCount.Text = _weibo.CommentsCount;
            RepostCount.Text = _weibo.RepostsCount;
            myItems.ItemsSource = _weiboSource;
        }

        private void RetweetButton_Click(object sender, RoutedEventArgs e)
        {
            Weibo _weiboParameter = _weibo;
            Frame.Navigate(typeof(RepostPage), _weiboParameter);
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            Weibo _weiboParameter = _weibo;
            Frame.Navigate(typeof(CommentPage), _weiboParameter);
        }

        private void replyButton_Click(object sender, RoutedEventArgs e)
        {
            Comment item = (Comment)(sender as Button).DataContext;
            Frame.Navigate(typeof(CommentPage), item);
        }


        private async void CommentItemRootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            string Uri = "https://api.weibo.com/2/comments/show.json?access_token=";
            Uri += App.access_token;
            Uri += "&id=";
            Uri += _weibo.ID;
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            commentuti = JsonConvert.DeserializeObject<CommentUti>(strResponse);
            CommentListView.ItemsSource = commentuti.Comment;
        }

        private async void MentionsItemRootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            string Uri = "https://api.weibo.com/2/statuses/repost_timeline.json?access_token=";
            Uri += App.weico_access_token;
            Uri += "&id=";
            Uri += _weibo.ID;
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            repostUti = JsonConvert.DeserializeObject<RepostUti>(strResponse);
            MentionListView.ItemsSource = repostUti.Repost;
        }
    }
}
