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
            CommentListview.ItemsSource = commentsource;
            MentionsListview.ItemsSource = mentionssource;
        }

        private void OnCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var pointx2 = Window.Current.CoreWindow.PointerPosition.X;
            if (pointx2 > pointx1)
            {
                if (myPivot.SelectedIndex == 0)
                    myPivot.SelectedIndex = 1;
                else
                    myPivot.SelectedIndex = 0;
            }
            else
                return;
        }

        private void OnStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            pointx1 = Window.Current.CoreWindow.PointerPosition.X;
        }
    }
}
