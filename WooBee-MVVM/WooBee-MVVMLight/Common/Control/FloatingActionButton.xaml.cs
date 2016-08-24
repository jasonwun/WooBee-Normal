using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WooBee_MVVMLight.ViewModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WooBee_MVVMLight
{
    public sealed partial class FloatingActionButton : UserControl
    {

        private TimeLineViewModel MainVM
        {
            get
            {
                TimeLineViewModel TimeLineVM = this.DataContext as TimeLineViewModel;

                if (TimeLineVM != null)
                {
                    TimeLineVM.NotificationShowAync += MainVM_NotificationShowAync;
                    TimeLineVM.MessageDisable += TimeLineVM_MessageDisable;
                    TimeLineVM.FollowerDisable += TimeLineVM_FollowerDisable;
                }
                    
                return TimeLineVM;
            }
        }

        private void TimeLineVM_FollowerDisable()
        {
            UserButton.Content = UserSymbol;
            UserButton.Background = new SolidColorBrush(Colors.Black);
        }

        private void TimeLineVM_MessageDisable()
        {
            MessageButton.Content = MessageSymbol;
            MessageButton.Background = new SolidColorBrush(Colors.Black);
        }

        public FloatingActionButton()
        {
            this.InitializeComponent();
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _popUp = ElementCompositionPreview.GetElementVisual(PopUp);
        }

        private void MainVM_NotificationShowAync()
        {
            if (MainVM.Notification.Noti_Total != "0")
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = MainVM.Notification.Noti_Total;
                textBlock.FontWeight = FontWeights.Bold;
                FloatingButton.Content = textBlock;
                FloatingButton.Background = new SolidColorBrush(Colors.Yellow);
            }
            else if(MainVM.Notification.Noti_Total == "0")
            {
                FloatingButton.Content = SymbolIcons;
                FloatingButton.Foreground = new SolidColorBrush(Colors.White);
                FloatingButton.Background = new SolidColorBrush(Colors.Black);
            }

        }

        private async void FloatingButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!IsOpened)
            {
                if(MainVM.Notification != null && MainVM.Notification.Follower != 0)
                {
                    UserNotificationShow();
                }
                if(MainVM.Notification != null && (MainVM.Notification.Cmt + MainVM.Notification.Mention_status) != 0)
                {
                    MessageNotificationShow();
                }
                FloatingButton.Content = SymbolIcons;
                FloatingButton.Background = new SolidColorBrush(Colors.Black);
                PopUp.Visibility = Visibility.Visible;
                ScalarKeyFrameAnimation opacityAnim = PopUpOpacityShowUpAnimation();
                ScalarKeyFrameAnimation offsetAnim = PopUpOffsetShowUpAnimation();

                _popUp.StartAnimation("Opacity", opacityAnim);
                _popUp.StartAnimation("Offset.Y", offsetAnim);


                IsOpened = true;
            }

            else
            {
                if (MainVM.Notification != null && MainVM.Notification.Noti_Total != "0")
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = MainVM.Notification.Noti_Total;
                    textBlock.FontWeight = FontWeights.Bold;
                    FloatingButton.Content = textBlock;
                    FloatingButton.Background = new SolidColorBrush(Colors.Yellow);
                }
                ScalarKeyFrameAnimation offsetAnim = PopUpOffsetDisableAnimation();
                ScalarKeyFrameAnimation opacityAnim = PopUpOpacityDisableAnimation();


                _popUp.StartAnimation("Opacity", opacityAnim);
                _popUp.StartAnimation("Offset.Y", offsetAnim);

                await Task.Delay(TimeSpan.FromMilliseconds(300));

                IsOpened = false;

                PopUp.Visibility = Visibility.Collapsed;
            }

        }

        private void MessageNotificationShow()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = MainVM.Notification.GetMessage;
            textBlock.FontWeight = FontWeights.Bold;
            MessageButton.Content = textBlock;
            MessageButton.Background = new SolidColorBrush(Colors.Yellow);
        }

        private void UserNotificationShow()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = MainVM.Notification.GetFollower;
            textBlock.FontWeight = FontWeights.Bold;
            UserButton.Content = textBlock;
            UserButton.Background = new SolidColorBrush(Colors.Yellow);
        }

        #region PopUpAnimation
        private ScalarKeyFrameAnimation PopUpOffsetShowUpAnimation()
        {
            var offsetAnim = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnim.InsertKeyFrame(0.0f, 3.0f);
            offsetAnim.InsertKeyFrame(1.0f, 0.0f);
            offsetAnim.Duration = TimeSpan.FromMilliseconds(200);
            return offsetAnim;
        }
        private ScalarKeyFrameAnimation PopUpOpacityShowUpAnimation()
        {
            var opacityAnim = _compositor.CreateScalarKeyFrameAnimation();
            opacityAnim.InsertKeyFrame(0.0f, 0.5f);
            opacityAnim.InsertKeyFrame(1.0f, 1.0f);
            opacityAnim.Duration = TimeSpan.FromMilliseconds(150);
            return opacityAnim;
        }
        private ScalarKeyFrameAnimation PopUpOffsetDisableAnimation()
        {
            var offsetAnim = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnim.InsertKeyFrame(0.0f, 0.0f);
            offsetAnim.InsertKeyFrame(1.0f, 3.0f);
            offsetAnim.Duration = TimeSpan.FromMilliseconds(200);
            return offsetAnim;
        }
        private ScalarKeyFrameAnimation PopUpOpacityDisableAnimation()
        {
            var opacityAnim = _compositor.CreateScalarKeyFrameAnimation();
            opacityAnim.InsertKeyFrame(0.0f, 1.0f);
            opacityAnim.InsertKeyFrame(1.0f, 0.0f);
            opacityAnim.Duration = TimeSpan.FromMilliseconds(150);
            return opacityAnim;
        } 
        #endregion


        bool IsOpened = false;
        Compositor _compositor;
        Visual _popUp;
    }
}
