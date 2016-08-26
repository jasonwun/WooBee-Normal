using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_MVVMLight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewPostView : BindablePage
    {

        #region Field
        public NewPostViewModel NewPostVM { get; set; }
        InputPane inputPane = InputPane.GetForCurrentView();
        private static Dictionary<string, string> _reverseEmojiDict { get; set; }
        private bool _isEmojiActivated = false;
        private bool _isInputPanelActivated = false;
        private string posttype = "";
        private string WeiboID = "";
        private string CommentID = "";
        #endregion

        #region Method
        private async void DisableStatusBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = StatusBar.GetForCurrentView();
                await statusbar.HideAsync();
            }
        }

        private void EmojiPanelShowing()
        {
            ButtonsPanel.Margin = new Thickness(0, 0, 10, 15 + Root.Height);
            ImagePanel.Margin = new Thickness(15, 0, 0, Root.Height);
            Root.Visibility = Visibility.Visible;

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
            _isEmojiActivated = true;
        }

        private void EmojiPanelHiding()
        {
            ButtonsPanel.Margin = new Thickness(0, 0, 10, 15);
            ImagePanel.Margin = new Thickness(15, 0, 0, 0);
            Root.Visibility = Visibility.Collapsed;
            _isEmojiActivated = false;
        }

        private void ReverseDict(Dictionary<string, string> a)
        {
            if (App._reverseDict.Count != 0)
            {
                _reverseEmojiDict = App._reverseDict;
                return;
            }
                
            foreach (var item in a)
            {
                App._reverseDict.Add(item.Value, item.Key);
            }
            _reverseEmojiDict = App._reverseDict;
        }

        void InputPaneHiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            ButtonsDown();
            if (_isEmojiActivated)
            {
                EmojiPanelShowing();
            }
        }
        private void InputPaneShowing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            if (_isEmojiActivated)
            {
                EmojiPanelHiding();
                ButtonsUp(args);
                _isEmojiActivated = false;
            }
            else
            {
                ButtonsUp(args);
            }
        }

        private void ButtonsUp(InputPaneVisibilityEventArgs args)
        {
            ButtonsPanel.Margin = new Thickness(0, 0, 10, 15 + args.OccludedRect.Height);
            ImagePanel.Margin = new Thickness(15, 0, 0, args.OccludedRect.Height);
        }

        private void ButtonsDown()
        {
            ButtonsPanel.Margin = new Thickness(0, 0, 10, 15);
            ImagePanel.Margin = new Thickness(15, 0, 0, 0);
        }
        #endregion

        #region Event
        private void UploadPhotoButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UploadPhotoButton.Visibility = Visibility.Collapsed;
            EmojiButton.Visibility = Visibility.Collapsed;
            SendButton.Visibility = Visibility.Collapsed;
            ReturnButton.Visibility = Visibility.Visible;
            CameraButton.Visibility = Visibility.Visible;
            PhotoButton.Visibility = Visibility.Visible;
        }

        private void EmojiButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!_isEmojiActivated)
            {
                _isEmojiActivated = true;
                EmojiPanelShowing();
            }

            else
            {
                _isEmojiActivated = false;
                EmojiPanelHiding();
            }
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (_isEmojiActivated)
            {
                EmojiPanelHiding();
                //e.Handled = true;
            }
        }

        private void ReturnButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ReturnButton.Visibility = Visibility.Collapsed;
            CameraButton.Visibility = Visibility.Collapsed;
            PhotoButton.Visibility = Visibility.Collapsed;
            UploadPhotoButton.Visibility = Visibility.Visible;
            EmojiButton.Visibility = Visibility.Visible;
            SendButton.Visibility = Visibility.Visible;
            inputPane.Visible = false;
        }

        private void NewPostVM_NewPhotosInserted()
        {
            ImagePanel.Visibility = Visibility.Visible;
        }

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var qwe = e.Parameter as PostWeiboNaviParamContainer;
            if(qwe.PostType == "post")
            {
                posttype = "post";
            }
            else if(qwe.PostType == "comment")
            {
                posttype = "comment";
                WeiboID = qwe.WeiboID;
            }
            else if(qwe.PostType == "repost")
            {
                posttype = "repost";
                WeiboID = qwe.WeiboID;
            }
            else if(qwe.PostType == "replycomment")
            {
                posttype = "replycomment";
                WeiboID = qwe.WeiboID;
                CommentID = qwe.CommentID;
            }
            else if(qwe.PostType == "replyrepost")
            {
                posttype = "replyrepost";
                WeiboID = qwe.WeiboID;
            }
            DataContext = NewPostVM = new NewPostViewModel(posttype, WeiboID, CommentID);
            if (qwe.PostType == "comment" || qwe.PostType == "repost" || qwe.PostType == "replycomment"
                || qwe.PostType == "replyrepost")
                NewPostVM.IsPhotoButtonShown = false;
            else
                NewPostVM.IsPhotoButtonShown = true;
            NewPostVM.NewPhotosInserted += NewPostVM_NewPhotosInserted;
        }

        public NewPostView()
        {
            this.InitializeComponent();
            DisableStatusBar();
            ReverseDict(App.emojiDict);
            inputPane.Showing += this.InputPaneShowing;
            inputPane.Hiding += this.InputPaneHiding;
            
        }

        private void EmoticonContainer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var _gridView = EmoticonContainer as GridView;
            string index = _gridView.SelectedIndex.ToString();
            if (Int32.Parse(index) < 10)
                index = "0" + index;

            NewPostVM.TextBlockString = NewPostVM.TextBlockString + _reverseEmojiDict[index];
        }

        private void contentTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            contentTextBox.Foreground = new SolidColorBrush(Colors.Black);
            contentTextBox.Text = "";
        }

        private void contentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(contentTextBox.Text == "")
            {
                contentTextBox.Text = "写点啥吧";
                contentTextBox.Foreground = new SolidColorBrush(Colors.LightGray);
            }
            else if(contentTextBox.Text == "写点啥吧")
                contentTextBox.Foreground = new SolidColorBrush(Colors.LightGray);
        }
    }
}
