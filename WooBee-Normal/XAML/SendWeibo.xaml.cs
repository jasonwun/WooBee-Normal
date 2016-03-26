using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
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
using Windows.UI.Xaml.Media.Imaging;
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
        private bool _isEmojiActivated = false;
        private static ObservableCollection<BitmapImage> _emojisource = App._emoticonSource;
        private static Dictionary<string, string> _reverseEmojiDict = new Dictionary<string, string>();

        public SendWeibo()
        {
            this.InitializeComponent();
            ShowStatusBar();
            ReverseDict(App.emojiDict);
            inputPane.Showing += this.InputPaneShowing;
            inputPane.Hiding += this.InputPaneHiding;
            message.Commands.Add(new UICommand("日"));
            EmoticonContainer.ItemsSource = _emojisource;
        }

        #region Field
        SolidColorBrush black = new SolidColorBrush(Windows.UI.Colors.Black);
        SolidColorBrush grey = new SolidColorBrush(Windows.UI.Colors.Gray);
        public event RoutedEventHandler LostFocus;
        MessageDialog message = new MessageDialog("该功能尚未开放");
        InputPane inputPane = InputPane.GetForCurrentView();
        #endregion

        #region Event

        private void EmoticonContainer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var _gridview = EmoticonContainer as GridView;
            var p = _gridview.SelectedItem;
            BitmapImage item = (BitmapImage)_gridview.SelectedItem;
            string uri = item.UriSource.ToString();
            uri = Regex.Match(uri, @"\d+").Value;
            if (contentTextBox.Text != "写点啥吧")
                contentTextBox.Text += _reverseEmojiDict[uri];
            else
            {
                contentTextBox.Text = "";
                contentTextBox.Foreground = black;
                contentTextBox.Text += _reverseEmojiDict[uri];
            }
        }


        private void contentTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
            if (contentTextBox.Text == "写点啥吧")
            {
                contentTextBox.Text = "";
                contentTextBox.Foreground = black;
            }

            if (_isEmojiActivated)
                EmojiPanelHiding();
        }

        void InputPaneHiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            this.SendButton.Margin = new Thickness(0, 0, 0, 20);
            this.EmojiButton.Margin = new Thickness(0, 0, 65, 20);
            this.UploadPhotoButton.Margin = new Thickness(0, 0, 130, 20);
            if (_isEmojiActivated)
                EmojiPanelShowing();
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
            if (!_isEmojiActivated)
            {
                EmojiPanelShowing();
            }

            else
            {
                EmojiPanelHiding();
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

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (_isEmojiActivated)
            {
                EmojiPanelHiding();
                e.Handled = true;
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
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            _isEmojiActivated = true;
        }

        private void EmojiPanelHiding()
        {
            SendButton.Margin = new Thickness(0, 0, 0, 20);
            EmojiButton.Margin = new Thickness(0, 0, 65, 20);
            UploadPhotoButton.Margin = new Thickness(0, 0, 130, 20);
            Nothing.Visibility = Visibility.Collapsed;
            _isEmojiActivated = false;
        }

        private void ReverseDict(Dictionary<string, string> a)
        {
            if (_reverseEmojiDict.Count != 0)
                return;
            foreach (var item in a)
            {
                _reverseEmojiDict.Add(item.Value, item.Key);
            }
        }


        #endregion

        private void DelteButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(contentTextBox.Text != "写点啥吧" && contentTextBox.Text.Length != 0)
                contentTextBox.Text = contentTextBox.Text.Substring(0, contentTextBox.Text.Length - 1);
        }
    }
}
