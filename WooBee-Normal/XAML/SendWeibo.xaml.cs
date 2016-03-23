using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_Normal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SendWeibo : Page
    {
        private bool _isEmojiButtomTapped = false;

        public SendWeibo()
        {
            this.InitializeComponent();
            ShowStatusBar();
            inputPane.Showing += this.InputPaneShowing;
            inputPane.Hiding += this.InputPaneHiding;
            message.Commands.Add(new UICommand("日"));
            
        }

        #region Field
        SolidColorBrush black = new SolidColorBrush(Windows.UI.Colors.Black);
        SolidColorBrush grey = new SolidColorBrush(Windows.UI.Colors.Gray);
        public event RoutedEventHandler LostFocus;
        MessageDialog message = new MessageDialog("该功能尚未开放");
        InputPane inputPane = InputPane.GetForCurrentView();
        #endregion

        #region Event
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
            this.SendButton.Margin = new Thickness(0, 0, 0, 20);
            this.EmojiButton.Margin = new Thickness(0, 0, 65, 20);
            this.UploadPhotoButton.Margin = new Thickness(0, 0, 130, 20);
        }

        private void InputPaneShowing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            this.SendButton.Margin = new Thickness(0, 0, 0, 20 + args.OccludedRect.Height);
            double itm = args.OccludedRect.Height;
            this.EmojiButton.Margin = new Thickness(0, 0, 65, 20 + args.OccludedRect.Height);
            this.UploadPhotoButton.Margin = new Thickness(0, 0, 130, 20 + args.OccludedRect.Height);
        }

        private async void SendButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string abc = contentTextBox.Text;
            if (abc == "写点啥吧" || abc == "")
            {
                var dialog = new Windows.UI.Popups.MessageDialog("不能为空");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                var result = await dialog.ShowAsync();
                contentTextBox.Foreground = grey;
                contentTextBox.Text = "写点啥吧";


            }
            else if (abc.Length > 140)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("超过了140个字啦！！！！！");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
                var result = await dialog.ShowAsync();
            }
            else
            {
                contentTextBox.LostFocus += LostFocus;
                await HttpPost(contentTextBox.Text);
                await Task.Delay(500);
                Frame.GoBack();
            }

        }

        private void EmojiButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isEmojiButtomTapped)
            {
                EmojiPanelShowing();
            }

            else
            {
                Nothing.LostFocus += Nothing_LostFocus;
            }

        }


        private async void UploadPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            await message.ShowAsync();
        }

        private void contentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (contentTextBox.Text == "")
            {
                contentTextBox.Foreground = grey;
                contentTextBox.Text = "写点啥吧";
            }

        }
        #endregion

        #region Method
        public static async Task HttpPost(string postMsg)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string posturi = "https://api.weibo.com/2/statuses/update.json?";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(posturi));
                HttpFormUrlEncodedContent postData = new HttpFormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("access_token",App.access_token),
                        new KeyValuePair<string, string>("status", postMsg),

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

        private async void ShowStatusBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = StatusBar.GetForCurrentView();
                await statusbar.HideAsync();
            }
        }

        private void EmojiPanelShowing()
        {
            SendButton.Margin = new Thickness(0, 0, 0, 20 + Nothing.Height);
            EmojiButton.Margin = new Thickness(0, 0, 65, 20 + Nothing.Height);
            UploadPhotoButton.Margin = new Thickness(0, 0, 130, 20 + Nothing.Height);
            Nothing.Visibility = Visibility.Visible;
            _isEmojiButtomTapped = true;
        }

        private void EmojiPanelHiding()
        {
            SendButton.Margin = new Thickness(0, 0, 0, 20);
            EmojiButton.Margin = new Thickness(0, 0, 65, 20);
            UploadPhotoButton.Margin = new Thickness(0, 0, 130, 20);
            Nothing.Visibility = Visibility.Collapsed;
            _isEmojiButtomTapped = false;
        }




        #endregion

        #region Protected

        #endregion

        private void Nothing_GotFocus(object sender, RoutedEventArgs e)
        {
            EmojiPanelShowing();
        }

        private void Nothing_LostFocus(object sender, RoutedEventArgs e)
        {
            EmojiPanelHiding();
        }
    }
}
