using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WooBee_MVVM.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_MVVMLight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PhotoPage : Page
    {

        public ObservableCollection<BitmapImage> _imageSource { get; set; }
        private int _index { get; set; }
        public PhotoPage()
        {
            this.InitializeComponent();
        }

        private async void ShowStatusBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = StatusBar.GetForCurrentView();
                await statusbar.HideAsync();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var p = e.Parameter as MultiImgNavigationParam;
            ObservableCollection<BitmapImage> _myimageSource = new ObservableCollection<BitmapImage>();
            if (p._imgSource != null)
            {
                foreach (var item in p._imgSource)
                {
                    if (item.Thumbpic.Substring(item.Thumbpic.Length - 3) == "gif")
                    {
                        string HighResGif = item.Thumbpic.Replace("thumbnail", "large");
                        BitmapImage tmp = new BitmapImage(new Uri(HighResGif));
                        _myimageSource.Add(tmp);
                    }
                    else
                    {
                        string HighResImg = item.Thumbpic.Replace("thumbnail", "mw1024");
                        BitmapImage tmp = new BitmapImage(new Uri(HighResImg));
                        _myimageSource.Add(tmp);
                    }

                }
            }

            if (p.ImgIndex >= 0)
                _index = p.ImgIndex;

            _imageSource = _myimageSource;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (_index > 0)
                ImagesFlip.SelectedIndex = _index;
        }
    }
}
