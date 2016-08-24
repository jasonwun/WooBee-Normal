using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WooBee_MVVM.Model;

namespace WooBee_MVVMLight
{
    public class NavigationParameter
    {
        public string PostParameter { get; set; }
        
        public string PostType { get; set; }

        public NavigationParameter(string param, string type)
        {
            PostParameter = param;
            PostType = type;
        }
    }

    public class MultiImgNavigationParam
    {
        public ObservableCollection<ThumbnailPics> _imgSource;
        public int ImgIndex;
    }

    public class PostWeiboNaviParamContainer
    {
        public string PostType="";
        public string WeiboID = "";
        public string CommentID = "";
    }
}
