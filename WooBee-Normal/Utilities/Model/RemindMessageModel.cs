using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooBee_Normal
{
    [System.Runtime.Serialization.DataContract]
    public class RemindMessageModel
    {
        [System.Runtime.Serialization.DataMember(Name = "status")]
        public int Status { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "follower")]
        public int Follower { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "cmt")]
        public int Cmt { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "dm")]
        public int Dm { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "mention_status")]
        public int Mention_status { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "mention_cmt")]
        public int Mention_cmt { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "notice")]
        public int Notice { set; get; }

    }
}
