using System;
using System.Collections.Generic;
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
    public sealed partial class Message : Page
    {
        CommentSource commentsource = new CommentSource();
        MentionsSource mentionssource = new MentionsSource();
        public double pointx1;
        public Message()
        {
            this.InitializeComponent();
        }

        private void replyButton_Click(object sender, RoutedEventArgs e)
        {
            Comment item = (Comment)(sender as Button).DataContext;
            Frame.Navigate(typeof(CommentPage), item);
            
        }
    }
}
