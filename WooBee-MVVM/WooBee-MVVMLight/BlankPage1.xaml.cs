using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WooBee_MVVMLight.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WooBee_MVVMLight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        public double ImageSize { get; set; }
        public BlankPage1()
        {
            this.InitializeComponent();
            ScalingHelper scalingHelper = new ScalingHelper();
            double a = scalingHelper.SetTimeLineMultiImgsSize();
            ObservableCollection<string> ew = new ObservableCollection<string>();
            ew.Add("http://ww3.sinaimg.cn/mw690/6ced65e4gw1f6nqg9dud1j20zk0qon5l.jpg");
            ew.Add("http://ww3.sinaimg.cn/mw690/6ced65e4gw1f6nqg9dud1j20zk0qon5l.jpg");
            ew.Add("http://ww3.sinaimg.cn/mw690/6ced65e4gw1f6nqg9dud1j20zk0qon5l.jpg");
            ew.Add("http://ww3.sinaimg.cn/mw690/6ced65e4gw1f6nqg9dud1j20zk0qon5l.jpg");
            ew.Add("http://ww3.sinaimg.cn/mw690/6ced65e4gw1f6nqg9dud1j20zk0qon5l.jpg");
            ew.Add("http://ww3.sinaimg.cn/mw690/6ced65e4gw1f6nqg9dud1j20zk0qon5l.jpg");
            ew.Add("http://ww3.sinaimg.cn/mw690/6ced65e4gw1f6nqg9dud1j20zk0qon5l.jpg");
            ew.Add("http://ww3.sinaimg.cn/mw690/6ced65e4gw1f6nqg9dud1j20zk0qon5l.jpg");
            ew.Add("http://ww3.sinaimg.cn/mw690/6ced65e4gw1f6nqg9dud1j20zk0qon5l.jpg");
            gridView.ItemsSource = ew;
        }



    }
}
