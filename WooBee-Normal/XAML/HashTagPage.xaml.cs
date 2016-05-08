using Newtonsoft.Json;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_Normal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HashTagPage : Page
    {
        
        public HashTagPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string a = "#" + e.Parameter.ToString() + "#";
            HashTag.Text = a;
            GetHashTagStatuses(e.Parameter.ToString());
        }

        private async void GetHashTagStatuses(string hashTagString)
        {
            string Uri = "https://api.weibo.com/2/search/topics.json?";
            Uri += "&access_token=";
            Uri += App.weico_access_token;
            Uri += "&q=";
            Uri += hashTagString;
            Uri += "&count=50";
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            var homeweibo = JsonConvert.DeserializeObject<HomeWeibo>(strResponse);
            HashTagListView.ItemsSource = homeweibo.Statuses;
        }
    }
}
