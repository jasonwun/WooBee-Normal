using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WooBee_MVVM.Model
{
    [DataContract]
    public class NotificationModel
    {
        [DataMember(Name = "status")]
        public int Status { set; get; }

        [DataMember(Name = "follower")]
        public int Follower { set; get; }

        [DataMember(Name = "cmt")]
        public int Cmt { set; get; }

        [DataMember(Name = "dm")]
        public int Dm { set; get; }

        [DataMember(Name = "mention_status")]
        public int Mention_status { set; get; }

        [DataMember(Name = "mention_cmt")]
        public int Mention_cmt { set; get; }

        [DataMember(Name = "notice")]
        public int Notice { set; get; }
    }
}
