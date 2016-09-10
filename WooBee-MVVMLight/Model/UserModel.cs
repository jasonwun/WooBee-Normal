using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

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
        [DataMember(Name = "idstr")]
        public string ID { set; get; }

        [DataMember(Name = "screen_name")]
        public string ScreenName { set; get; }

        [DataMember(Name = "location")]
        public string Location { set; get; }

        [DataMember(Name = "cover_image")]
        public string CoverImage { set; get; }

        [DataMember(Name = "cover_image_phone")]
        public string CoverImagePhone { set; get; }

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
        public bool Following { set; get; }

        [DataMember(Name = "verified")]
        public string Verified { set; get; }

        [DataMember(Name = "allow_all_comment")]
        public string AllowAllComment { set; get; }

        [DataMember(Name = "avatar_large")]
        public string AvatarLarge { set; get; }

        [DataMember(Name = "verified_reason")]
        public string VerifiedReason { set; get; }

        [DataMember(Name = "follow_me")]
        public bool FollowMe { set; get; }

        [IgnoreDataMember]
        public string FriendShipStatus
        {
            get
            {
                if (Following && FollowMe)
                    return "互相关注";
                else if (Following)
                    return "已关注";
                else
                    return "未关注";
            }
        }

        [IgnoreDataMember]
        public BitmapImage IsMaleOrFemale
        {
            get
            {
                if (Gender == "m")
                    return new BitmapImage(new Uri("ms-appx:///Assets/Icons/male-icon.jpg"));
                else
                    return new BitmapImage(new Uri("ms-appx:///Assets/Icons/female-icon.png"));
            }
        }

        [IgnoreDataMember]
        public string FollwersCountOutput
        {
            get
            {
                return "粉丝数: " + FollowersCount;
            }
        }

        [IgnoreDataMember]
        public string FriendsCountCountOutput
        {
            get
            {
                return "关注数: " + FriendsCount;
            }
        }

        [IgnoreDataMember]
        public string WeiboCountOutput
        {
            get
            {
                return "微博数: " + StatusesCount;
            }
        }
    }
}
