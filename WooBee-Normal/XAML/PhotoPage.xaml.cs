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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_Normal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PhotoPage : Page
    {

        private static ObservableCollection<ThumbnailPics> _imageSource { get; set; }

        public PhotoPage()
        {
            this.InitializeComponent();
            ShowStatusBar();
            
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
            _imageSource = e.Parameter as ObservableCollection<ThumbnailPics>;
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        { 
            foreach( var item in _imageSource)
            {
                item.thumpic.Replace("thumbnail", "mw1024");
            }
            ImagesFlip.ItemsSource = _imageSource;
        }
    }
}
