using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WooBee_MVVMLight.ViewModel;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using WooBee_MVVMLight.View;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_MVVMLight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginView : Page
    {
        public LoginViewModel Vm
        {
            get
            {
                return (LoginViewModel)DataContext;
            }
        }

        public LoginView()
        {
            this.InitializeComponent();
            DataContext = new LoginViewModel();
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(App.AccessToken != "")
            {
                Vm.NormalButtonEnabled = false;
            }
            else
            {
                Vm.NormalButtonEnabled = true;
            }
            if (App.WeicoAccessToken != "")
            {
                Vm.ExtendedButtonEnabled = false;
            }
            else
            {
                Vm.ExtendedButtonEnabled = true;
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.AccessToken != "" && App.WeicoAccessToken != "")
            {
                ContentDialog aa = new ContentDialog();
                TextBlock text = new TextBlock();
                text.Text = "正在处理中";
                text.Margin = new Thickness(90);
                aa.Content = text;
                aa.VerticalAlignment = VerticalAlignment.Center;
                aa.HorizontalAlignment = HorizontalAlignment.Center;
                var p =  aa.ShowAsync();
                await Task.Delay(2500);
                p.Cancel();
                Frame.Navigate(typeof(TimeLineView));
            }
        }
    }
}
