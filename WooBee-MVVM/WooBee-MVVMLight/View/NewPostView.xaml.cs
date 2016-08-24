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
        private static Dictionary<string, string> _reverseEmojiDict = new Dictionary<string, string>();
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
            SendButton.Margin = new Thickness(0, 0, 0, 20 + Root.Height);
            EmojiButton.Margin = new Thickness(0, 0, 65, 20 + Root.Height);
            UploadPhotoButton.Margin = new Thickness(0, 0, 130, 20 + Root.Height);
            PhotoButton.Margin = new Thickness(0, 0, 0, 20 + Root.Height);
            CameraButton.Margin = new Thickness(0, 0, 65, 20 + Root.Height);
            ReturnButton.Margin = new Thickness(0, 0, 130, 20 + Root.Height);
            //ImagePanel.Margin = new Thickness(0, 0, 0, Root.Height);
            Root.Visibility = Visibility.Visible;

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
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
            //ImagePanel.Margin = new Thickness(0, 0, 0, 0);
            Root.Visibility = Visibility.Collapsed;
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

        void InputPaneHiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            SendButton.Margin = new Thickness(0, 0, 0, 20);
            EmojiButton.Margin = new Thickness(0, 0, 65, 20);
            UploadPhotoButton.Margin = new Thickness(0, 0, 130, 20);
            PhotoButton.Margin = new Thickness(0, 0, 0, 20);
            CameraButton.Margin = new Thickness(0, 0, 65, 20);
            ReturnButton.Margin = new Thickness(0, 0, 130, 20);
            //ImagePanel.Margin = new Thickness(0, 0, 0, 0);
            _isInputPanelActivated = false;
            if (_isEmojiActivated)
                EmojiPanelShowing();
        }

        private void InputPaneShowing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            //EmojiPanelHiding();
            SendButton.Margin = new Thickness(0, 0, 0, 20 + args.OccludedRect.Height);
            PhotoButton.Margin = new Thickness(0, 0, 0, 20 + args.OccludedRect.Height);
            EmojiButton.Margin = new Thickness(0, 0, 65, 20 + args.OccludedRect.Height);
            CameraButton.Margin = new Thickness(0, 0, 65, 20 + args.OccludedRect.Height);
            UploadPhotoButton.Margin = new Thickness(0, 0, 130, 20 + args.OccludedRect.Height);
            ReturnButton.Margin = new Thickness(0, 0, 130, 20 + args.OccludedRect.Height);
            //ImagePanel.Margin = new Thickness(0, 0, 0, args.OccludedRect.Height);
            _isInputPanelActivated = true;

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
                EmojiPanelShowing();
            }

            else
            {
                EmojiPanelHiding();
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

            //TODO: Animation
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

        //TODO: 重新整理表情页面跟键盘之间的显示消失的逻辑
    }
}
