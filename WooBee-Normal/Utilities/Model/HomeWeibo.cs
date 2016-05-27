using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WooBee_Normal
{
    /// <summary>
    /// 保存微博集合数据类
    /// </summary>
    [DataContract]
    public class HomeWeibo
    {
        //微博集合数据
        [DataMember(Name = "statuses")]
        public IList<Weibo> Statuses { set; get; }

        [DataMember(Name = "previous_cursor")]
        public string PreviousCursor { set; get; }

        [DataMember(Name = "next_cursor")]
        public string NestCursor { set; get; }

        [DataMember(Name = "total_number")]
        public string TotalNumber { set; get; }
    }
}
