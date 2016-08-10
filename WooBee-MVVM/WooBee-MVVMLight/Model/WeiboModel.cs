using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
        public string ThumbnailPic { set; get; }

        [DataMember(Name = "pic_urls")]
        public ObservableCollection<ThumbnailPics> PicUrls { set; get; }
    }

    [DataContract]
    public class ThumbnailPics
    {
        [DataMember(Name = "thumbnail_pic")]
        public string Thumbpic { set; get; }
    }

    
}
