using Newtonsoft.Json.Linq;
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
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
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
        private bool _isEmojiActivated = false;
        private static ObservableCollection<BitmapImage> _emojisource = App._emoticonSource;
        private static Dictionary<string, string> _reverseEmojiDict = new Dictionary<string, string>();
        private static IReadOnlyList<StorageFile> files { get; set; }
        private static string pic_ids = "";
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
            this.PhotoButton.Margin = new Thickness(0, 0, 0, 20);
            this.CameraButton.Margin = new Thickness(0, 0, 65, 20);
            this.ReturnButton.Margin = new Thickness(0, 0, 130, 20);
            if (_isEmojiActivated)
                EmojiPanelShowing();
        }

        private void InputPaneShowing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            this.SendButton.Margin = new Thickness(0, 0, 0, 20 + args.OccludedRect.Height);
            this.PhotoButton.Margin = new Thickness(0, 0, 0, 20 + args.OccludedRect.Height);
            this.EmojiButton.Margin = new Thickness(0, 0, 65, 20 + args.OccludedRect.Height);
            this.CameraButton.Margin = new Thickness(0, 0, 65, 20 + args.OccludedRect.Height);
            this.UploadPhotoButton.Margin = new Thickness(0, 0, 130, 20 + args.OccludedRect.Height);
            this.ReturnButton.Margin = new Thickness(0, 0, 130, 20 + args.OccludedRect.Height);


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
                if(files.Count == 0)
                {
                    contentTextBox.LostFocus += LostFocus;
                    await HttpPost(contentTextBox.Text);
                    await Task.Delay(500);
                    Frame.GoBack();
                }
                else if(files.Count == 1)
                {
                    if (abc == "写点啥吧")
                        abc = "分享图片";
                    await HttpPostWithImage(abc);
                    Frame.GoBack();
                }

                else if(files.Count > 1)
                {
                    if (contentTextBox.Text == "写点啥吧")
                        contentTextBox.Text = "分享图片";
                    await HttpPostWithMultipleImage(abc);
                    Frame.GoBack();
                }
                
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

        private void UploadPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            UploadPhotoButton.Visibility = Visibility.Collapsed;
            EmojiButton.Visibility = Visibility.Collapsed;
            SendButton.Visibility = Visibility.Collapsed;
            ReturnButton.Visibility = Visibility.Visible;
            CameraButton.Visibility = Visibility.Visible;
            PhotoButton.Visibility = Visibility.Visible;
            inputPane.Visible = false;
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

        private void DelteButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (contentTextBox.Text != "写点啥吧" && contentTextBox.Text.Length != 0)
                contentTextBox.Text = contentTextBox.Text.Substring(0, contentTextBox.Text.Length - 1);
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnButton.Visibility = Visibility.Collapsed;
            CameraButton.Visibility = Visibility.Collapsed;
            PhotoButton.Visibility = Visibility.Collapsed;
            UploadPhotoButton.Visibility = Visibility.Visible;
            EmojiButton.Visibility = Visibility.Visible;
            SendButton.Visibility = Visibility.Visible;
            inputPane.Visible = false;
        }

        private async void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            await message.ShowAsync();
        }

        private async void PhotoButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            open.ViewMode = PickerViewMode.Thumbnail;

            // Filter to include a sample subset of file types
            open.FileTypeFilter.Clear();
            open.FileTypeFilter.Add(".gif");
            open.FileTypeFilter.Add(".png");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".jpg");

            // Open a stream for the selected file
            files = await open.PickMultipleFilesAsync();
            if(files != null)
            {
                if(files.Count == 1)
                {
                    IRandomAccessStream fileStream = await files[0].OpenAsync(FileAccessMode.Read);
                    BitmapImage bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(fileStream);
                    image.Source = bitmapImage;
                    ImagesCount.Visibility = Visibility.Collapsed;
                    ImagePanel.Visibility = Visibility.Visible;
                }
                 else if(files.Count > 1)
                {
                    IRandomAccessStream fileStream = await files[0].OpenAsync(FileAccessMode.Read);
                    BitmapImage bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(fileStream);
                    image.Source = bitmapImage;
                    ImagesCount.Text = files.Count.ToString() ;
                    ImagesCount.Visibility = Visibility.Visible;
                    ImagePanel.Visibility = Visibility.Visible;
                }
                
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

        private static async Task HttpPostWithImage(string postMsg)
        {
            string posturi = "https://upload.api.weibo.com/2/statuses/upload.json?access_token=" + App.access_token;
            HttpClient httpClient = new HttpClient();
            FileRandomAccessStream stream = (FileRandomAccessStream)await files[0].OpenAsync(FileAccessMode.Read);
            HttpStreamContent streamContent1 = new HttpStreamContent(stream);
            HttpStringContent stringContent = new HttpStringContent(postMsg);
            HttpMultipartFormDataContent fileContent = new HttpMultipartFormDataContent();
            fileContent.Add(stringContent, "status");
            fileContent.Add(streamContent1, "pic", "pic.jpg");
            HttpResponseMessage response = await httpClient.PostAsync(new Uri(posturi), fileContent);
            string responString = await response.Content.ReadAsStringAsync();
        }

        private async Task HttpPostWithMultipleImage(string abc)
        {
            string posturi = "https://api.weibo.com/2/statuses/upload_url_text.json?access_token=" + App.weico_access_token;
            foreach (StorageFile file in files)
            {
                FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);
                HttpStreamContent streamContent1 = new HttpStreamContent(stream);
                await UploadImages(streamContent1);

            }
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(posturi));
            HttpFormUrlEncodedContent postData = new HttpFormUrlEncodedContent(
                new List<KeyValuePair<string, string>>
                {
                        new KeyValuePair<string, string>("status", abc),
                        new KeyValuePair<string, string>("pic_id", pic_ids),

                }
            );
            request.Content = postData;
            HttpResponseMessage response = await httpClient.SendRequestAsync(request);
            string responseString = await response.Content.ReadAsStringAsync();
        }

        private async Task UploadImages(HttpStreamContent streamContent)
        {
            string posturi = "https://upload.api.weibo.com/2/statuses/upload_pic.json?access_token=" + App.weico_access_token;
            HttpClient httpClient = new HttpClient();
            HttpMultipartFormDataContent fileContent = new HttpMultipartFormDataContent();
            fileContent.Add(streamContent, "pic", "pic.jpg");
            HttpResponseMessage response = await httpClient.PostAsync(new Uri(posturi), fileContent);
            string a = await response.Content.ReadAsStringAsync();
            JObject o = JObject.Parse(a);
            pic_ids += (string)o["pic_id"];
            pic_ids += ",";
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
            PhotoButton.Margin = new Thickness(0, 0, 0, 20 + Nothing.Height);
            CameraButton.Margin = new Thickness(0, 0, 65, 20 + Nothing.Height);
            ReturnButton.Margin = new Thickness(0, 0, 130, 20 + Nothing.Height);
            Nothing.Visibility = Visibility.Visible;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            _isEmojiActivated = true;
        }

        private void EmojiPanelHiding()
        {
            SendButton.Margin = new Thickness(0, 0, 0, 20);
            EmojiButton.Margin = new Thickness(0, 0, 65, 20);
            UploadPhotoButton.Margin = new Thickness(0, 0, 130, 20);
            PhotoButton.Margin = new Thickness(0, 0, 0, 20);
            CameraButton.Margin = new Thickness(0, 0, 65, 20);
            ReturnButton.Margin = new Thickness(0, 0, 130, 20);
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

        
    }
}
