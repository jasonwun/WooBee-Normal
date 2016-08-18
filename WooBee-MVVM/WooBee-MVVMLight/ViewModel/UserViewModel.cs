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

        private string _iD;
        public string ID
        {
            get
            {
                return _iD;
            }
            set
            {
                if (_iD != value)
                    _iD = value;
                RaisePropertyChanged(() => ID);
            }
        }

        public double ButtonWidth
        {
            get
            {
                ScalingHelper scale = new ScalingHelper();
                double tmp = scale.GetScreenWidth() / 3;
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
            await UserFollowingDataViewModel.RefreshAsync();
            await UserWeiboDataViewModel.RefreshAsync();
            UserProfile = await GetUserProfile(ID);
            FollowerList = UserFollowerDataViewModel.DataList;
            FollowingList = UserFollowingDataViewModel.DataList;
            WeiboList = UserWeiboDataViewModel.DataList;
            IsRefreshing = false;

        }


        private bool IsRefreshing;
        public bool IsFirstActived { get; set; } = true;

        private static async Task<User> GetUserProfile(string ID)
        {
            string Uri = API.USER_SHOW;
            Uri += "?access_token=";
            Uri += App.WeicoAccessToken;
            Uri += "&screen_name=";
            Uri += ID;

            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            User _userprofiles = JsonConvert.DeserializeObject<User>(strResponse);
            return _userprofiles;
        }

        public UserViewModel(string iD)
        {
            ID = iD;
            UserWeiboDataViewModel = new UserWeiboDataViewModel(ID);
            UserFollowerDataViewModel = new UserFollowerDataViewModel(ID);
            UserFollowingDataViewModel = new UserFollowingDataViewModel(ID);
            WeiboList = new ObservableCollection<Weibo>();
            FollowingList = new ObservableCollection<User>();
            FollowerList = new ObservableCollection<User>();
        }
    }
}
