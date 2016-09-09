using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using WooBee_MVVMLight;

namespace WooBee_MVVM.Model
{
    [DataContract]
    public class WeiboModel
    {
        [DataMember(Name = "statuses")]
        public ObservableCollection<Weibo> Statuses { get; set; }
    }

    [DataContract]
    public class RepostModel
    {
        [DataMember(Name = "reposts")]
        public ObservableCollection<Weibo> Repost { get; set; }
    }


    [DataContract]
    public class Weibo
    {
        [DataMember(Name = "created_at")]
        public string CreatedAt { set; get; }

        [DataMember(Name = "id")]
        public string ID { set; get; }

        [DataMember(Name = "text")]
        public string Text { set; get; }

        [DataMember(Name = "favorited")]
        public string Favorited { set; get; }

        [DataMember(Name = "geo")]
        public object Geo { set; get; }

        [DataMember(Name = "reposts_count")]
        public string RepostsCount { set; get; }

        [DataMember(Name = "comments_count")]
        public string CommentsCount { set; get; }

        [DataMember(Name = "attitudes_count")]
        public string AttitudesCount { set; get; }

        [DataMember(Name = "user")]
        public User User { set; get; }

        [DataMember(Name = "retweeted_status")]
        public Weibo RepostWeibo { set; get; }

        [DataMember(Name = "thumbnail_pic")]
        public string ThumbnailPic { get; set; }

        [DataMember(Name = "pic_urls")]
        public ObservableCollection<ThumbnailPics> PicUrls { set; get; }

        [IgnoreDataMember]
        public BitmapImage HighResThumnailPic
        {
            get
            {
                if(ThumbnailPic != null)
                {
                    string HighRes = ThumbnailPic.Replace("thumbnail", "mw690");
                    BitmapImage a = new BitmapImage(new Uri(HighRes));
                    a.AutoPlay = false;
                    return a;
                }
                else
                {
                    return new BitmapImage();
                }
                
            }
        }

        [IgnoreDataMember]
        public bool IsGif
        {
            get
            {
                
                if ( ThumbnailPic != null && ThumbnailPic.Substring(ThumbnailPic.Length - 3) == "gif")
                    return true;
                else
                    return false;
            }
        }

        [IgnoreDataMember]
        public string RepostText
        {
            get
            {
                if (Text == "抱歉，此微博已被作者删除。查看帮助：http://t.cn/zWSudZc")
                    return Text;
                else
                {
                    return "@" + User.ScreenName + ": " + Text;
                }
            }
        }
    }

    [DataContract]
    public class ThumbnailPics
    {
        [DataMember(Name = "thumbnail_pic")]
        public string Thumbpic { get; set; }

        [IgnoreDataMember]
        public double ImgSize
        {
            get
            {
                ScalingHelper scalingHelper = new ScalingHelper();
                return scalingHelper.SetTimeLineMultiImgsSize();
            }
        }

        [IgnoreDataMember]
        public BitmapImage HighResThumPic
        {
            get
            {
                string HighRes = Thumbpic.Replace("thumbnail", "mw690");
                BitmapImage a = new BitmapImage(new Uri(HighRes));
                a.AutoPlay = false;
                return a;
            }
        }

        [IgnoreDataMember]
        public bool IsGif
        {
            get
            {
                if (Thumbpic.Substring(Thumbpic.Length - 3) == "gif")
                    return true;
                else
                    return false;
            }
        }

    }

    
}
