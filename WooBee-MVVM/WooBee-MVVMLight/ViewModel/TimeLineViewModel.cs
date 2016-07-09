using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WooBee_MVVM.Model;
using System.ComponentModel;

namespace WooBee_MVVMLight.ViewModel
{
    public class TimeLineViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public static int DEFAULT_PAGE_INDEX => 1;
        public static uint DEFAULT_PER_PAGE { get; set; } = 20u;

        private int PageIndex { get; set; } = DEFAULT_PAGE_INDEX;


    }
}
