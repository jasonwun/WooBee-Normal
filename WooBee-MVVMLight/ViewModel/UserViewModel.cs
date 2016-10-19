using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using WooBee_MVVM.Model;
using WooBee_MVVMLight.Common;

namespace WooBee_MVVMLight
{
    public class UserViewModel : ViewModelBase, INavigable
    {

        private UserWeiboDataViewModel _userWeiboDataViewModel;
        public UserWeiboDataViewModel UserWeiboDataViewModel
        {
            get
            {
                return _userWeiboDataViewModel;
            }
            set
            {
                if (_userWeiboDataViewModel != value)
                    _userWeiboDataViewModel = value;
                RaisePropertyChanged(() => UserWeiboDataViewModel);
            }
        }

        private UserFollowerDataViewModel _userFollowerDataViewModel;
        public UserFollowerDataViewModel UserFollowerDataViewModel
        {
            get
            {
                return _userFollowerDataViewModel;
            }
            set
            {
                if (_userFollowerDataViewModel != value)
                    _userFollowerDataViewModel = value;
                RaisePropertyChanged(() => UserFollowerDataViewModel);
            }
        }

        private UserFollowingDataViewModel _userFollowingDataViewModel;
        public UserFollowingDataViewModel UserFollowingDataViewModel
        {
            get
            {
                return _userFollowingDataViewModel;
            }
            set
            {
                if (_userFollowingDataViewModel != value)
                    _userFollowingDataViewModel = value;
                RaisePropertyChanged(() => UserFollowingDataViewModel);
            }
        }

        private ObservableCollection<Weibo> _weiboList;
        public ObservableCollection<Weibo> WeiboList
        {
            get
            {
                return _weiboList;
            }
            set
            {
                if (_weiboList != value)
                    _weiboList = value;
                RaisePropertyChanged(() => WeiboList);
            }
        }

        private ObservableCollection<User> _followerList;
        public ObservableCollection<User> FollowerList
        {
            get
            {
                return _followerList;
            }
            set
            {
                if (_followerList != value)
                    _followerList = value;
                RaisePropertyChanged(() => FollowerList);
            }
        }

        private ObservableCollection<User> _followingList;
        public ObservableCollection<User> FollowingList
        {
            get
            {
                return _followingList;
            }
            set
            {
                if (_followingList != value)
                    _followingList = value;
                RaisePropertyChanged(() => FollowingList);
            }
        }

        private User _userProfile;
        public User UserProfile
        {
            get
            {
                return _userProfile;
            }
            set
            {
                if (_userProfile != value)
                    _userProfile = value;
                RaisePropertyChanged(() => UserProfile);
            }
        }

        private string _param;
        public string Param
        {
            get
            {
                return _param;
            }
            set
            {
                if (_param != value)
                    _param = value;
                RaisePropertyChanged(() => Param);
            }
        }

        public double ImageWidth
        {
            get
            {
                ScalingHelper scale = new ScalingHelper();
                double tmp = scale.GetWindowWidth() * 0.83;
                return tmp;
            }
        }

        public double ButtonHeight
        {
            get
            {
                ScalingHelper scale = new ScalingHelper();
                double tmp = scale.GetWindowsHeight() * 0.04;
                return tmp;
            }
        }

        public double ButtonWidth
        {
            get
            {
                ScalingHelper scale = new ScalingHelper();
                double tmp = scale.GetWindowWidth() * 0.17;
                return tmp;
            }
        }


        public void Activate(object parameter)
        {
            
        }

        public void Deactivate(object parameter)
        {
        }

        public async void OnLoaded()
        {
            if (IsFirstActived)
            {
                IsFirstActived = false;
                await RefreshAsync();
            }
        }

        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            await UserFollowerDataViewModel.RefreshAsync();
            FollowerList = UserFollowerDataViewModel.DataList;
            await UserFollowingDataViewModel.RefreshAsync();
            FollowingList = UserFollowingDataViewModel.DataList;
            UserProfile = await GetUserProfile(Param, posttype);
            IsRefreshing = false;
        }


        private bool IsRefreshing;
        public bool IsFirstActived { get; set; } = true;
        private string posttype { get; set; }

        private static async Task<User> GetUserProfile(string param, string posttype)
        {
            string Uri = API.USER_SHOW;
            Uri += "?access_token=";
            Uri += App.WeicoAccessToken;
            if (posttype == "uid")
                Uri += "&uid=";
            else if (posttype == "screen_name")
                Uri += "&screen_name=";
            Uri += param;

            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            User _userprofiles = JsonConvert.DeserializeObject<User>(strResponse);
            return _userprofiles;
        }

        public UserViewModel(string param, string type)
        {
            Param = param;
            posttype = type;
            UserWeiboDataViewModel = new UserWeiboDataViewModel(Param, posttype);
            UserFollowerDataViewModel = new UserFollowerDataViewModel(Param, posttype);
            UserFollowingDataViewModel = new UserFollowingDataViewModel(Param, posttype);
            WeiboList = new ObservableCollection<Weibo>();
            FollowingList = new ObservableCollection<User>();
            FollowerList = new ObservableCollection<User>();
        }
    }
}
