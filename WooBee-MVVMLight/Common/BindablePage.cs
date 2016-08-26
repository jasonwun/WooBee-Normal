using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace WooBee_MVVMLight
{
    public class BindablePage: Page
    {
        public event EventHandler<KeyEventArgs> GlobalPageKeyDown;

        public BindablePage()
        {
            IsTextScaleFactorEnabled = false;
            this.Loaded += BindablePage_Loaded;
        }

        private void BindablePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is INavigable)
            {
                (this.DataContext as INavigable).OnLoaded();
            }
        }


        protected virtual void SetNavigationBackBtn()
        {
            if (this.Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        /// <summary>
        /// 全局下按下按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            GlobalPageKeyDown(sender, args);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (DataContext is INavigable)
            {
                var NavigationViewModel = (INavigable)DataContext;
                if (NavigationViewModel != null)
                {
                    NavigationViewModel.Activate(e.Parameter);
                }
            }

            SetNavigationBackBtn();

            Window.Current.SetTitleBar(null);

            //resolve global keydown
            if (GlobalPageKeyDown != null)
            {
                Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            }
            //UmengSDK.UmengAnalytics.TrackPageStart(this.GetType().ToString());
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (DataContext is INavigable)
            {
                var NavigationViewModel = (INavigable)DataContext;
                if (NavigationViewModel != null)
                {
                    NavigationViewModel.Deactivate(null);
                }
            }

            //resolve global keydown
            if (GlobalPageKeyDown != null)
            {
                Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            }
            //UmengSDK.UmengAnalytics.TrackPageEnd(this.GetType().ToString());
        }
    }
}
