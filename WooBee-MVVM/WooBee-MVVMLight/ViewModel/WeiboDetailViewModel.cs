using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooBee_MVVM.Model;

namespace WooBee_MVVMLight
{
    public class WeiboDetailViewModel: ViewModelBase
    {
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
                    RaisePropertyChanged(() => Weibo);
                }
            }
        }


        public WeiboDetailViewModel() { }


    }
}
