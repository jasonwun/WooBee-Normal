using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace WooBee_Normal
{
    /// <summary>
    /// 微博信息类
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class Weibo
    {
        
        [System.Runtime.Serialization.DataMember(Name = "created_at")]
        public string CreatedAt { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "id")]
        public string ID { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "text")]
        public string Text { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "source")]
        public string Source { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "favorited")]
        public string Favorited { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "truncated")]
        public string Truncated { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "in_reply_to_status_id")]
        public string InReplyToStatusId { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "in_reply_to_user_id")]
        public string InReplyToUserId { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "in_reply_to_screen_name")]
        public string InReplyToScreenName { set; get; }

        //[System.Runtime.Serialization.DataMember(Name = "geo")]
        //public string Geo { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "mid")]
        public string MID { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "reposts_count")]
        public string RepostsCount { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "comments_count")]
        public string CommentsCount { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "attitudes_count")]
        public string AttitudesCount { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "user")]
        public User User { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "retweeted_status")]
        public RepostWeibo RepostWeibo { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "thumbnail_pic")]
        public string ThumbnailPic{ set; get; }

        [System.Runtime.Serialization.DataMember(Name = "pic_urls")]
        public ObservableCollection<ThumbnailPics> PicUrls { set; get; }

        
        //public Paragraph RichText
        //{
        //    get
        //    {
        //        ConverRichText(Text);
        //    }
        //}

        //private Paragraph ConverRichText(string text)
        //{
        //    Paragraph richpara = ;
        //    return richpara;
        //}
    }
}
