using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using WooBee_MVVM.Model;
using WooBee_MVVMLight.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_MVVMLight.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimeLineView: BindablePage
    {

        public TimeLineViewModel TLVm { get; set; }

        public TimeLineView()
        {
            this.InitializeComponent();
            this.DataContext = TLVm = new TimeLineViewModel();
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

        private void listView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var _listview = listView as ListView;
            var _weibo = (Weibo)_listview.SelectedItem;
            ListViewItemPresenter originalsource = e.OriginalSource as ListViewItemPresenter;
            if (originalsource == null)
            {
                var advancesource = e.OriginalSource as TextBlock;
                var imageSource = e.OriginalSource as Image;
                var richtextBlock = e.OriginalSource as RichTextBlock;
                if (advancesource != null)
                {
                    if (advancesource.Text == "网页链接")
                    {
                        e.Handled = true;
                    }
                    else if (advancesource.Text.Substring(0, 1) == "#")
                    {
                        string hashtag = advancesource.Text.Substring(1);
                        hashtag = hashtag.Remove(hashtag.Length - 1);
                        //Frame.Navigate(typeof(HashTagPage), hashtag);
                    }
                    else if (advancesource.Text.Substring(0, 1) == "@")
                    {
                        string username = advancesource.Text.Substring(1);
                        Frame.Navigate(typeof(UserView), username);
                    }
                    else if (_weibo.User.ScreenName != null && advancesource.Text == _weibo.User.ScreenName)
                    {
                        string username = advancesource.Text;
                        Frame.Navigate(typeof(UserView), username);
                    }
                    else if(advancesource.Text == _weibo.RepostWeibo.User.ScreenName)
                    {
                        string username = advancesource.Text;
                        Frame.Navigate(typeof(UserView), username);
                    }
                    else
                    {
                        Frame.Navigate(typeof(WeiboDetailView), _weibo);
                    }
                }

                else if (imageSource != null)
                {
                    string username = _weibo.User.ScreenName;
                    Frame.Navigate(typeof(UserView), username);
                }
                else if (richtextBlock != null)
                {
                    Frame.Navigate(typeof(WeiboDetailView), _weibo);
                }


            }

            else
            {
                Frame.Navigate(typeof(WeiboDetailView), _weibo);
            }


        }

    }
}
