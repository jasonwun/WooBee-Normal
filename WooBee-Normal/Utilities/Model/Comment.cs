using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooBee_Normal
{
    [System.Runtime.Serialization.DataContract]
    public class Comment
    {
        [System.Runtime.Serialization.DataMember(Name = "created_at")]
        public string CreatedAt { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "id")]
        public string ID { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "text")]
        public string Text { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "user")]
        public User User { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "status")]
        public Weibo Status { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "reply_comment")]
        public RepliedComment ReplyComment { set; get; }
    }
}
