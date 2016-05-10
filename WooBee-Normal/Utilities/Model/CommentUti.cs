using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooBee_Normal
{
    [System.Runtime.Serialization.DataContract]
    public class CommentUti
    {
        [System.Runtime.Serialization.DataMember(Name = "comments")]
        public IList<Comment> Comment { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "previous_cursor")]
        public string PreviousCursor { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "next_cursor")]
        public string NestCursor { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "total_number")]
        public string TotalNumber { set; get; }
    }

}
