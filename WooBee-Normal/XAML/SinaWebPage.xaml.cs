using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_Normal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SinaWebPage : Page
    {
        private static string NormalUri = "https://api.weibo.com/oauth2/authorize" + "?client_id=" + App.client_id + "&redirect_uri=" + OauthSina.RedirectUri;
        private static string WeicoUri = "https://open.weibo.cn/oauth2/authorize?client_id=";
        private static string appkey = "211160679";
        private static string appsecret = "1e6e33db08f9192306c4afa0a61ad56c";
        private static string scope = "email,direct_messages_read,direct_messages_write,friendships_groups_read,friendships_groups_write,statuses_to_me_read,follow_app_official_microblog,invitation_write";
        private static ApplicationDataContainer localsetting = ApplicationData.Current.LocalSettings;
        string code;
        OauthSina oauthsina = new OauthSina();

        public SinaWebPage()
        {
            this.InitializeComponent();
            GetWeicoString();
        }

        private void GetWeicoString()
        {
            WeicoUri += appkey;
            WeicoUri += "&response_type=token&redirect_uri=";
            WeicoUri += OauthSina.RedirectUri;
            WeicoUri += "&key_hash=";
            WeicoUri += appsecret;
            WeicoUri += "&packagename=com.eico.weico&display=mobile&scope=";
            WeicoUri += scope;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.ToString() == "normal")
                LoginWebView.Navigate(new Uri(NormalUri, UriKind.Absolute));
            else if (e.Parameter.ToString() == "weico")
                LoginWebView.Navigate(new Uri(WeicoUri, UriKind.Absolute));
        }

        private async void LoginWebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            Debug.WriteLine(args.Uri.ToString());
            if (args.Uri.AbsoluteUri.ToString().Contains("code"))
            {
                Debug.WriteLine(args.Uri.ToString());
                code = args.Uri.AbsoluteUri.ToString();
                code = code.Substring(code.IndexOf("=") + 1);
                oauthsina.SetCode(code);
                await oauthsina.GetAccessToekn();
                Frame.GoBack();
            }
            else if (args.Uri.AbsoluteUri.ToString().Contains("access_token"))
            {
                string WeicoAccessToken = args.Uri.AbsoluteUri.ToString();
                WeicoAccessToken = WeicoAccessToken.Substring(WeicoAccessToken.IndexOf("=") + 1, 32);
                App.weico_access_token = WeicoAccessToken;
                App.localsetting.Values["weico_access_token"] = WeicoAccessToken;
                Frame.GoBack();
            }
        }
    }
}
