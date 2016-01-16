using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooBee_Normal
{
    [System.Runtime.Serialization.DataContract]
    public class User
    {
        [System.Runtime.Serialization.DataMember(Name = "id")]
        public string ID { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "screen_name")]
        public string ScreenName { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "name")]
        public string Name { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "province")]
        public string Province { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "city")]
        public string City { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "location")]
        public string Location { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "description")]
        public string Description { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "url")]
        public string Url { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "profile_image_url")]
        public string ProfileImageUrl { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "domain")]
        public string Domain { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "gender")]
        public string Gender { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "followers_count")]
        public string FollowersCount { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "friends_count")]
        public string FriendsCount { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "statuses_count")]
        public string StatusesCount { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "favourites_count")]
        public string FavouritesCount { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "created_at")]
        public string CreatedAt { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "following")]
        public string Following { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "allow_all_act_msg")]
        public string AllowAllActMsg { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "remark")]
        public string Remark { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "geo_enabled")]
        public string GeoEnabled { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "verified")]
        public string Verified { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "allow_all_comment")]
        public string AllowAllComment { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "avatar_large")]
        public string AvatarLarge { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "verified_reason")]
        public string VerifiedReason { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "follow_me")]
        public string FollowMe { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "online_status")]
        public string OnlineStatus { set; get; }

        [System.Runtime.Serialization.DataMember(Name = "bi_followers_count")]
        public string BiFollowersCount { set; get; }
    }
}
