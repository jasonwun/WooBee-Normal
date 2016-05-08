using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI;
using System.Globalization;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_Normal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserPage : Page
    { 
        private string _userName { get; set; }
        private UserProfiles _userprofiles { get; set; }
        private ObservableCollection<Weibo> StatusSource = new ObservableCollection<Weibo>();
        private ObservableCollection<User> FollowersSource = new ObservableCollection<User>();
        private ObservableCollection<User> FriendsSource = new ObservableCollection<User>();
        private BitmapImage MaleIcon = new BitmapImage(new Uri("ms-appx:///Assets/Icons/male-icon.jpg"));
        private BitmapImage FemaleIcon = new BitmapImage(new Uri("ms-appx:///Assets/Icons/female-icon.png"));
        private SolidColorBrush HighlightBrush { get; set; }
        private SolidColorBrush NormalBrush { get; set; }

        public UserPage()
        {
            this.InitializeComponent();
            SetHighLightBrush();
            SetNormalBrush();
        }

        private void SetNormalBrush()
        {
            string NormalHex = "#FFEAEAEA";
            byte a = byte.Parse(NormalHex.Substring(1, 2), NumberStyles.HexNumber);
            byte r = byte.Parse(NormalHex.Substring(3, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(NormalHex.Substring(5, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(NormalHex.Substring(7, 2), NumberStyles.HexNumber);

            Color color = Color.FromArgb(a, r, g, b);
            SolidColorBrush br = new SolidColorBrush(color);
            NormalBrush = br;
        }

        private void SetHighLightBrush()
        {
            string HighlightHex = "#FFAFAFAF";
            byte a = byte.Parse(HighlightHex.Substring(1, 2), NumberStyles.HexNumber);
            byte r = byte.Parse(HighlightHex.Substring(3, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(HighlightHex.Substring(5, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(HighlightHex.Substring(7, 2), NumberStyles.HexNumber);

            Color color = Color.FromArgb(a, r, g, b);
            SolidColorBrush br = new SolidColorBrush(color);
            HighlightBrush = br;
        }

        private async void GetUserProfile()
        {
            string Uri = "https://api.weibo.com/2/users/show.json?";
            Uri += "&access_token=";
            Uri += App.weico_access_token;
            Uri += "&screen_name=";
            Uri += _userName;

            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            //UserProfiles _userprofiles = new UserProfiles();
            _userprofiles = JsonConvert.DeserializeObject<UserProfiles>(strResponse);
            BindProfiles();
            await StatuesSource();
            await FollowerSource();
            await FriendSource();

            Statues.ItemsSource = StatusSource;
            Followers.ItemsSource = FollowersSource;
            Follwings.ItemsSource = FriendsSource;
        }

        private async Task StatuesSource()
        {
            string Uri = "https://api.weibo.com/2/statuses/user_timeline.json?";
            Uri += "&access_token=";
            Uri += App.weico_access_token;
            Uri += "&screen_name=";
            Uri += _userprofiles.ScreenName;
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            var homeweibo = JsonConvert.DeserializeObject<HomeWeibo>(strResponse);
            foreach(var item in homeweibo.Statuses)
            {
                StatusSource.Add(item);
            }
        }

        private async Task FollowerSource()
        {
            string Uri = "https://api.weibo.com/2/friendships/followers.json?";
            Uri += "&access_token=";
            Uri += App.weico_access_token;
            Uri += "&screen_name=";
            Uri += _userprofiles.ScreenName;
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            var users = JsonConvert.DeserializeObject<UserSource>(strResponse);
            foreach (var item in users.Users)
            {
                FollowersSource.Add(item);
            }
        }

        private async Task FriendSource()
        {
            string Uri = "https://api.weibo.com/2/friendships/friends.json?";
            Uri += "&access_token=";
            Uri += App.weico_access_token;
            Uri += "&screen_name=";
            Uri += _userprofiles.ScreenName;
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            var users = JsonConvert.DeserializeObject<UserSource>(strResponse);
            foreach (var item in users.Users)
            {
                FriendsSource.Add(item);
            }
        }

        private void BindProfiles()
        {
            UserName.Text = _userprofiles.ScreenName;
            if (_userprofiles.CoverImage != null)
                CoverImage.Source = new BitmapImage(new Uri(_userprofiles.CoverImage));
            UserAvatar.Source = new BitmapImage(new Uri(_userprofiles.AvatarLarge));
            if (_userprofiles.Gender == "m")
                Gender.Source = MaleIcon;
            else
                Gender.Source = FemaleIcon;
            Location.Text = _userprofiles.Location;
            DetermineRelationship();
            WeiboCounts.Content = "微博: " + _userprofiles.StatusesCount.ToString();
            FollowerCounts.Content = "粉丝: " + _userprofiles.FollowersCount.ToString();
            FollwingCounts.Content = "关注: " + _userprofiles.FriendsCount.ToString();
        }

        private void DetermineRelationship()
        {
            if (_userprofiles.Following && !_userprofiles.FollowingMe)
                FollowingButton.Content = "已关注";
            else if (_userprofiles.Following && _userprofiles.FollowingMe)
                FollowingButton.Content = "互相关注";
            else if (!_userprofiles.Following && !_userprofiles.FollowingMe)
                FollowingButton.Content = "未关注";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _userName = e.Parameter.ToString();
            GetUserProfile();
        }

        private void WeiboCounts_Tapped(object sender, TappedRoutedEventArgs e)
        {

            WeiboCounts.Background = HighlightBrush;

            FollowerCounts.Background = NormalBrush;

            FollwingCounts.Background = NormalBrush;

            Statues.Visibility = Visibility.Visible;
            Followers.Visibility = Visibility.Collapsed;
            Follwings.Visibility = Visibility.Collapsed;
        }

        private void FollowerCounts_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FollowerCounts.Background = HighlightBrush;

            WeiboCounts.Background = NormalBrush;

            FollwingCounts.Background = NormalBrush;

            Statues.Visibility = Visibility.Collapsed;
            Followers.Visibility = Visibility.Visible;
            Follwings.Visibility = Visibility.Collapsed;

        }

        private void FollwingCounts_Tapped(object sender, TappedRoutedEventArgs e)
        {

            FollwingCounts.Background = HighlightBrush;

            WeiboCounts.Background = NormalBrush;

            FollowerCounts.Background = NormalBrush;

            Statues.Visibility = Visibility.Collapsed;
            Followers.Visibility = Visibility.Collapsed;
            Follwings.Visibility = Visibility.Visible;
        }

        private void FollowingButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_userprofiles.Following)
            {
                CalcelFollowing();
                _userprofiles.Following = false;
            }
                

            else if (!_userprofiles.Following)
            {
                Following();
                _userprofiles.Following = true;
            }

            DetermineRelationship();
                
        }

        private async void Following()
        {
            HttpClient httpClient = new HttpClient();
            string posturi = "https://api.weibo.com/2/friendships/create.json?";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(posturi));
            HttpFormUrlEncodedContent postData = new HttpFormUrlEncodedContent(
                new List<KeyValuePair<string, string>>
                {
                        new KeyValuePair<string, string>("access_token",App.weico_access_token),
                        new KeyValuePair<string, string>("screen_name", _userprofiles.ScreenName),
                }
            );
            request.Content = postData;
            HttpResponseMessage response = await httpClient.SendRequestAsync(request);
            string responseString = await response.Content.ReadAsStringAsync();
        }

        private async void CalcelFollowing()
        {
            HttpClient httpClient = new HttpClient();
            string posturi = "https://api.weibo.com/2/friendships/destroy.json?";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(posturi));
            HttpFormUrlEncodedContent postData = new HttpFormUrlEncodedContent(
                new List<KeyValuePair<string, string>>
                {
                        new KeyValuePair<string, string>("access_token",App.weico_access_token),
                        new KeyValuePair<string, string>("screen_name", _userprofiles.ScreenName),
                }
            );
            request.Content = postData;
            HttpResponseMessage response = await httpClient.SendRequestAsync(request);
            string responseString = await response.Content.ReadAsStringAsync();

        }
    }
}
