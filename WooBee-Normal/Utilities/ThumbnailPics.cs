using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooBee_Normal
{
    [System.Runtime.Serialization.DataContract]
    public class ThumbnailPics
    {
        [System.Runtime.Serialization.DataMember(Name = "thumbnail_pic")]
        public string thumpic { set; get; }
    }
}
