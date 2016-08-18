using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using WooBee_MVVMLight.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_MVVMLight.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebViewXAML : Page
    {
        public WebViewXAML()
        {
            this.InitializeComponent();
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
            
            if (e.Parameter.ToString() == "normal")
            {
                Uri NormalAuthorizeUri = new Uri(API.OAUTH2_ACCESS_AUTHORIZE + "?client_id=839927271&redirect_uri=http://oauth.weico.cc");
                Uri rr = NormalAuthorizeUri;
                WebView.Navigate(rr);
            }
            else
            {
                Uri requri = new Uri(API.OAUTH2_ACCESS_AUTHORIZE + "?client_id=211160679&response_type=token&redirect_uri=http://oauth.weico.cc&key_hash=1e6e33db08f9192306c4afa0a61ad56c&packagename=com.eico.weico&display=mobile&scope=email,direct_messages_read,direct_messages_write,friendships_groups_read,friendships_groups_write,statuses_to_me_read,follow_app_official_microblog,invitation_write");
                Uri rr = requri;
                WebView.Navigate(rr);
            }
            
            
        }

        private async void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri.AbsoluteUri.ToString().Contains("code"))
            {
                string code = args.Uri.AbsoluteUri.ToString();
                code = code.Substring(code.IndexOf("=") + 1);
                await GetAccessToken(code);
            }
            else if (args.Uri.AbsoluteUri.ToString().Contains("access_token"))
            {
                string WeicoAccessToken = args.Uri.AbsoluteUri.ToString();
                WeicoAccessToken = WeicoAccessToken.Substring(WeicoAccessToken.IndexOf("=") + 1, 32);
                App.WeicoAccessToken = WeicoAccessToken;
                Frame.GoBack();
            }
        }

        private async Task GetAccessToken(string code)
        {
            string AccessToken = "";
            try
            {
                HttpClient httpclient = new HttpClient();
                string posturi = "https://api.weibo.com/oauth2/access_token?";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(posturi));
                HttpFormUrlEncodedContent postData = new HttpFormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("client_id", "839927271"),
                        new KeyValuePair<string, string>("client_secret", "d9a2ae8a01ef87772897bcf0c32ea575"),
                        new KeyValuePair<string, string>("grant_type","authorization_code"),
                        new KeyValuePair<string, string>("redirect_uri", "http://oauth.weico.cc"),
                        new KeyValuePair<string, string>("code",code),

                    }
                );
                request.Content = postData;
                HttpResponseMessage response = await httpclient.SendRequestAsync(request);
                string responseString = await response.Content.ReadAsStringAsync();
                JsonObject token = JsonObject.Parse(responseString);
                AccessToken = token.GetNamedString("access_token").ToString();
                string Uid = token.GetNamedString("uid").ToString();
                App.AccessToken = AccessToken;
                App.Uid = Uid;
                Frame.GoBack();

            }
            catch
            {

            }
        }
    }
}
