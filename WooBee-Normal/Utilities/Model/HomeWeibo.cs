using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooBee_Normal
{
    /// <summary>
    /// 保存微博集合数据类
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class HomeWeibo
    {
        //微博集合数据
        [System.Runtime.Serialization.DataMember(Name = "statuses")]
        public IList<Weibo> Statuses { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "previous_cursor")]
        public string PreviousCursor { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "next_cursor")]
        public string NestCursor { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "total_number")]
        public string TotalNumber { set; get; }
    }
}
