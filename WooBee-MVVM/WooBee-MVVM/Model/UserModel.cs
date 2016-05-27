using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WooBee_MVVM.Model
{
    [DataContract]
    public class UserModel
    {
        [DataMember(Name = "users")]
        public ObservableCollection<User> Users { get; set; }
    }


    [DataContract]
    public class User
    {
        [DataMember(Name = "id")]
        public string ID { set; get; }

        [DataMember(Name = "screen_name")]
        public string ScreenName { set; get; }

        [DataMember(Name = "location")]
        public string Location { set; get; }

        [DataMember(Name = "description")]
        public string Description { set; get; }

        [DataMember(Name = "url")]
        public string Url { set; get; }

        [DataMember(Name = "profile_image_url")]
        public string ProfileImageUrl { set; get; }

        [DataMember(Name = "gender")]
        public string Gender { set; get; }

        [DataMember(Name = "followers_count")]
        public string FollowersCount { set; get; }

        [DataMember(Name = "friends_count")]
        public string FriendsCount { set; get; }

        [DataMember(Name = "statuses_count")]
        public string StatusesCount { set; get; }

        [DataMember(Name = "following")]
        public string Following { set; get; }

        [DataMember(Name = "verified")]
        public string Verified { set; get; }

        [DataMember(Name = "allow_all_comment")]
        public string AllowAllComment { set; get; }

        [DataMember(Name = "avatar_large")]
        public string AvatarLarge { set; get; }

        [DataMember(Name = "verified_reason")]
        public string VerifiedReason { set; get; }

        [DataMember(Name = "follow_me")]
        public string FollowMe { set; get; }
    }
}
