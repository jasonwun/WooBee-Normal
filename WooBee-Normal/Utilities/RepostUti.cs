using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooBee_Normal
{
    [System.Runtime.Serialization.DataContract]
    public class RepostUti
    {
        [System.Runtime.Serialization.DataMember(Name = "reposts")]
        public IList<Repost> Repost { set; get; }

    }
}
