using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WooBee_MVVM.Model;
using WooBee_MVVMLight.Common;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WooBee_MVVMLight.UserControlView
{
    public sealed partial class WeiboDetail : NavigableUserControl, INotifyPropertyChanged
    {
        public WeiboDetailViewModel WeiboDV { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private Weibo _weibo;
        public Weibo Weibo
        {
            get
            {
                return _weibo;
            }
            set
            {
                if(_weibo != value)
                {
                    _weibo = value;
                    RaisePropertyChanged(nameof(Weibo));
                }
            }
        }

        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public WeiboDetail()
        {
            this.InitializeComponent();
            this.DataContext = this;
            WeiboDV = new WeiboDetailViewModel();
        }

        public override void OnShow()
        {
            base.OnShow();
        }

        public override void OnHide()
        {
            Detail.SelectedIndex = 0;
            base.OnHide();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Shown = false;
        }
    }
}
