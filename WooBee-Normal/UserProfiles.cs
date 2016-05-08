using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooBee_Normal
{
    [System.Runtime.Serialization.DataContract]
    public class UserProfiles
    {
        [System.Runtime.Serialization.DataMember(Name = "id")]
        public string ID { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "screen_name")]
        public string ScreenName { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "location")]
        public string Location { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "description")]
        public string Description { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "avatar_large")]
        public string AvatarLarge { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "cover_image_phone")]
        public string CoverImage { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "gender")]
        public string Gender { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "followers_count")]
        public int FollowersCount { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "friends_count")]
        public int FriendsCount { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "statuses_count")]
        public int StatusesCount { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "following")]
        public bool Following { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "follow_me")]
        public bool FollowingMe { set; get; }
    }
}
