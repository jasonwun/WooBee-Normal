using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_Normal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        string Uri = "https://api.weibo.com/oauth2/authorize" + "?client_id=" + App.client_id + "&redirect_uri=" + OauthSina.RedirectUri;
        OauthSina oauthsina = new OauthSina();
        int count = 0;
        string code;

        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void webView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            string codestr = "code";
            if (args.Uri.AbsoluteUri.ToString().Contains(codestr))
            {
                Debug.WriteLine(args.Uri.ToString());
                code = args.Uri.AbsoluteUri.ToString();
                code = code.Substring(code.IndexOf("=") + 1);
                oauthsina.SetCode(code);
                await oauthsina.GetAccessToekn();
                Frame.Navigate(typeof(TimeLine));
                Debug.WriteLine(args.Uri.ToString());
            }
            else
                return;

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.localsetting.Values.ContainsKey("access_token"))
                webView.Navigate(new Uri(Uri, UriKind.Absolute));
            else
            {
                object value = App.localsetting.Values["access_token"];
                App.access_token = value.ToString();
                Frame.Navigate(typeof(TimeLine));
            }

        }
    }
}
