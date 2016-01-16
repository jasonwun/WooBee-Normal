using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_Normal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CommentPage : Page
    {
        SolidColorBrush black = new SolidColorBrush(Windows.UI.Colors.Black);
        SolidColorBrush grey = new SolidColorBrush(Windows.UI.Colors.Gray);
        public event RoutedEventHandler LostFocus;
        private static Weibo _weibo { get; set; }

        public CommentPage()
        {
            this.InitializeComponent();
            InputPane inputPane = InputPane.GetForCurrentView();
            ShowStatusBar();
            inputPane.Showing += this.InputPaneShowing;
            inputPane.Hiding += this.InputPaneHiding;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _weibo = e.Parameter as Weibo;
        }

        private async void SendButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string abc = contentTextBox.Text;
            if (abc == "写点啥吧" || abc=="")
            {
                var dialog = new Windows.UI.Popups.MessageDialog("不能为空");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                var result = await dialog.ShowAsync();
                contentTextBox.Text = "写点啥吧";
                contentTextBox.Foreground = grey;

            }
            else
            {
                contentTextBox.LostFocus += LostFocus;
                await HttpPost(contentTextBox.Text);
                await Task.Delay(500);
                Frame.GoBack();
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

        public static async Task HttpPost(string postMsg)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string posturi = "https://api.weibo.com/2/comments/create.json?";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(posturi));
                HttpFormUrlEncodedContent postData = new HttpFormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("access_token",App.access_token),
                        new KeyValuePair<string, string>("comment", postMsg),
                        new KeyValuePair<string, string>("id", _weibo.ID.ToString())

                    }
                );
                request.Content = postData;
                HttpResponseMessage response = await httpClient.SendRequestAsync(request);
                string responseString = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                string asc = ex.ToString();
            }
        }

        private void contentTextBox_GotFocus(object sender, RoutedEventArgs e)
        {

            if (contentTextBox.Text == "写点啥吧")
            {
                contentTextBox.Text = "";
                contentTextBox.Foreground = black;
            }
        }

        void InputPaneHiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            this.SendButton.Margin = new Thickness(0, 536, 0, 10);
        }

        private void InputPaneShowing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            this.SendButton.Margin = new Thickness(0, 536 - args.OccludedRect.Height, 0, 10 + args.OccludedRect.Height);
        }
    }
}
