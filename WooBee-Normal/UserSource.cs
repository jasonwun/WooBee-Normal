using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooBee_Normal
{
    [System.Runtime.Serialization.DataContract]
    public class UserSource
    {
        [System.Runtime.Serialization.DataMember(Name = "users")]
        public IList<User> Users { set; get; }
    }
}
