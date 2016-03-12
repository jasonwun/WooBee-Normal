﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class LoginPage : Page
    {


        public LoginPage()
        {
            this.InitializeComponent();
            if (App.access_token != null)
                NormalToken.IsChecked = true;
            if (App.weico_access_token != null)
                WeicoToken.IsChecked = true;
            if (NormalToken.IsChecked ==true && WeicoToken.IsChecked==true)
                NavigateButton.IsEnabled = true;
        }

        private void NavigateNormalToken_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SinaWebPage), "normal");
        }

        private void NavigateWeicoToken_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SinaWebPage), "weico");
        }

        private void NavigateButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TimeLine));
        }
    }
}
