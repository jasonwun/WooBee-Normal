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
            //Weibo weibo = JsonConvert.DeserializeObject<Weibo>(_passingString);
            ScreenNameTextBlock.Text = _weibo.User.ScreenName;
            //WeiboTextBlock.Text = _weibo.Text;
            var uri = new Uri(_weibo.User.AvatarLarge);
            var bitmap = new BitmapImage(uri);
            WeiboUserAvatar.Source = bitmap;
            _weiboSource.Add(_weibo);
            myItems.ItemsSource = _weiboSource;
        }

        private async void CommentListView_Loaded(object sender, RoutedEventArgs e)
        {
            string Uri = "https://api.weibo.com/2/comments/show.json?source=";
            Uri += App.client_id;
            Uri += "&access_token=";
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
    }
}
