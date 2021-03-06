﻿using System;
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
using WooBee_MVVMLight.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_MVVMLight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MessageView : BindablePage
    {
        public MessageViewModel MessageVm { get; set; }
        public MessageView()
        {
            this.InitializeComponent();
            this.DataContext = MessageVm = new MessageViewModel();
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

        private void replyButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PostWeiboNaviParamContainer a = new PostWeiboNaviParamContainer();
            Comment comment = (Comment)(sender as Button).DataContext;
            a.PostType = "replycomment";
            a.CommentID = comment.ID;
            a.WeiboID = comment.Status.ID;
            NavigationService.NaivgateToPage(typeof(NewPostView), a);
        }

        private void ReplyWeiboButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PostWeiboNaviParamContainer a = new PostWeiboNaviParamContainer();
            Weibo weibo = (Weibo)(sender as Button).DataContext;
            a.PostType = "replyrepost";
            a.WeiboID = weibo.ID;
            NavigationService.NaivgateToPage(typeof(NewPostView), a);
        }
    }
}
