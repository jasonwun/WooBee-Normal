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
        private int _index { get; set; }

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
            var p = e.Parameter as PageParametersContainers;
            if(p.parameter1 != null)
                _imageSource = p.parameter1;
            if(p.parameter2 >= 0)
                _index = p.parameter2;
            foreach (var item in _imageSource)
            {
                item.thumpic = item.thumpic.Replace("thumbnail", "mw1024");
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        { 
            ImagesFlip.ItemsSource = _imageSource;
            if(_index > 0)
                ImagesFlip.SelectedIndex = _index;
        }
    }
}
