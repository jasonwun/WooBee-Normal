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
    public class CommentModel
    {
        [DataMember(Name = "comments")]
        public ObservableCollection<Comment> Comments { get; set; }
    }

    [DataContract]
    public class Comment
    {
        [DataMember(Name = "created_at")]
        public string CreatedAt { set; get; }

        [DataMember(Name = "id")]
        public string ID { set; get; }

        [DataMember(Name = "text")]
        public string Text { set; get; }

        [DataMember(Name = "user")]
        public User User { set; get; }

        [DataMember(Name = "status")]
        public Weibo Status { set; get; }

        [DataMember(Name = "reply_comment")]
        public Comment ReplyComment { set; get; }
    }
}
