using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using XamlAnimatedGif;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_Normal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PhotoPage : Page
    {

        private static ObservableCollection<MyImage> _imageSource { get; set; }
        
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
            ObservableCollection<MyImage> _myimageSource = new ObservableCollection<MyImage>();
            if(p.parameter1 != null)
            {
                foreach (var item in p.parameter1)
                {
                    if(item.thumpic.Substring(item.thumpic.Length - 4) == ".gif")
                    {
                        item.thumpic = item.thumpic.Replace("thumbnail", "large");
                        MyImage tmp = new MyImage();
                        tmp.Image = item.thumpic;
                        _myimageSource.Add(tmp);
                    }
                    else
                    {
                        item.thumpic = item.thumpic.Replace("thumbnail", "mw1024");
                        MyImage tmp = new MyImage();
                        tmp.Image = item.thumpic;
                        _myimageSource.Add(tmp);
                    }
                    
                }
            }
                
            if(p.parameter2 >= 0)
                _index = p.parameter2;

            _imageSource=_myimageSource;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        { 
            ImagesFlip.ItemsSource = _imageSource;
            if(_index > 0)
                ImagesFlip.SelectedIndex = _index;
        }

        //private void photo_ImageOpened(object sender, RoutedEventArgs e)
        //{
            
        //    (((Image)sender).DataContext as MyImage).IsLoading = false;
        //}
    }

    public class MyImage : INotifyPropertyChanged
    {

        public MyImage()
        {
            this._IsLoading = true;
        }

        public string Image { get; set; }

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                
                _IsLoading = value;
                RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
