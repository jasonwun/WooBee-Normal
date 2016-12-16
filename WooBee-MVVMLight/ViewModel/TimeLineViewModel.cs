using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WooBee_MVVM.Model;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using WooBee_MVVMLight.Common;
using Windows.Web.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using Windows.Web.Http.Filters;
using Windows.Storage;

namespace WooBee_MVVMLight.ViewModel
{
    public class TimeLineViewModel : ViewModelBase, INavigable
    {

        private TimeLineDataViewModel _timeLineDataViewModel;
        public TimeLineDataViewModel TimeLineDataViewModel
        {
            get
            {
                return _timeLineDataViewModel;
            }
            set
            {
                if (_timeLineDataViewModel != value)
                {
                    _timeLineDataViewModel = value;
                    RaisePropertyChanged(() => TimeLineDataViewModel);
                }
            }
        }


        private ObservableCollection<Weibo> _mainList;
        public ObservableCollection<Weibo> MainList
        {
            get
            {
                return _mainList;
            }
            set
            {
                if (_mainList != value)
                {
                    _mainList = value;
                    RaisePropertyChanged(() => MainList);
                }
            }
        }

        private RelayCommand _goToSettingViewCommand;
        public RelayCommand GoToSettingViewCommand
        {
            get
            {
                if (_goToSettingViewCommand != null) return _goToSettingViewCommand;
                return _goToSettingViewCommand = new RelayCommand(() =>
                {
                    NavigationService.NaivgateToPage(typeof(SettingView));
                });
            }
        }

        private RelayCommand _goToUserViewCommand;
        public RelayCommand GoToUserViewCommand
        {
            get
            {
                if (_goToUserViewCommand != null) return _goToUserViewCommand;
                return _goToUserViewCommand = new RelayCommand(async () =>
                {
                    if (Notification.Follower != 0)
                    {
                        await ResetNotification("follower");
                    }
                    NavigationParameter navip = new NavigationParameter(App.Uid, "uid");
                    NavigationService.NaivgateToPage(typeof(UserView), navip);
                });
            }
        }

        private RelayCommand _goToMessageViewCommand;
        public RelayCommand GoToMessageViewCommand
        {
            get
            {
                if (_goToMessageViewCommand != null) return _goToMessageViewCommand;
                return _goToMessageViewCommand = new RelayCommand(async () =>
                {
                    if ((Notification.Cmt + Notification.Mention_status) != 0)
                    {
                        await ResetNotification("cmt");
                        await ResetNotification("mention_status");
                    }
                    NavigationService.NaivgateToPage(typeof(MessageView));
                });
            }
        }

        private RelayCommand _goToNewPostViewCommand;
        public RelayCommand GoToNewPostViewCommand
        {
            get
            {
                if (_goToNewPostViewCommand != null) return _goToNewPostViewCommand;
                return _goToNewPostViewCommand = new RelayCommand(() =>
                {
                    PostWeiboNaviParamContainer a = new PostWeiboNaviParamContainer();
                    a.PostType = "post";
                    NavigationService.NaivgateToPage(typeof(NewPostView),a);
                });
            }
        }

        private NotificationModel _notification;
        public NotificationModel Notification
        {
            get
            {
                return _notification;
            }
            set
            {
                if (_notification != value)
                    _notification = value;
                RaisePropertyChanged(() => Notification);
            }
        }

        private bool _showWeiboDetail;
        public bool ShowWeiboDetail
        {
            get
            {
                return _showWeiboDetail;
            }
            set
            {
                if (_showWeiboDetail != value)
                {
                    _showWeiboDetail = value;
                    RaisePropertyChanged(() => ShowWeiboDetail);
                }
            }
        }


        public TimeLineViewModel()
        {
        }

        #region Empty Methods

        public void Activate(object parameter) { }
        public void Deactivate(object parameter) { }
        #endregion


        public async void OnLoaded()
        {

            if (IsFirstActived)
            {
                IsFirstActived = false;
                await RestoreMainListDataAsync();
                MainList = TimeLineDataViewModel.DataList;
                await RefreshAllAsync();
                MainList = TimeLineDataViewModel.DataList;
            }
        }

        private async Task RefreshAllAsync()
        {
            await RefreshListAsync();
        }

        private async Task RestoreMainListDataAsync()
        {
            InitDataVM();
            var file = await CacheUtil.GetCachedFileFolder().TryGetItemAsync("MainList.list") as IStorageFile; 
            if (file != null)
            {
                var str = await FileIO.ReadTextAsync(file);
                var list = JsonConvert.DeserializeObject<List<Weibo>>(str);
                if (list != null)
                {
                    list.ForEach(s => TimeLineDataViewModel.DataList.Add(s));
                    return;
                }
            }
        }

        private void InitDataVM()
        {
            TimeLineDataViewModel = new TimeLineDataViewModel();
        }

        private async Task RefreshListAsync()
        {
            IsRefreshing = true;
            await TimeLineDataViewModel.RefreshAsync();
            IsRefreshing = false;
            await SaveMainListDataAsync();

        }

        public async Task StoreListAsync()
        {
            await SaveMainListDataAsync();
        }

        private async Task SaveMainListDataAsync()
        {
            if (this.TimeLineDataViewModel.DataList?.Count > 0)
            {
                var list = new List<Weibo>();
                foreach (var item in TimeLineDataViewModel.DataList)
                {
                    if (item is Weibo)
                    {
                        list.Add(item as Weibo);
                    }
                }
                if (list.Count > 0)
                {
                    //ToastService.SendToast("Fetched :D");

                    var str = JsonConvert.SerializeObject(list, new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("MainList.list", CreationCollisionOption.OpenIfExists);
                    await FileIO.WriteTextAsync(file, str);
                }
            }
        }

        public async void Refresh()
        {
            App.Since_id++;
            await RefreshAllAsync();
        }

        public async Task RefreshNotification()
        {
            try
            {
                await Task.Delay(1000);
                string Uri = API.REMIND_UNREAD_COUNT;
                Uri += "?source=211160679";
                Uri += "&access_token=";
                Uri += App.WeicoAccessToken;
                Uri += "&uid=";
                Uri += App.Uid.ToString();
                Uri += "&a=";
                Uri += new Random().Next().ToString();
                var httpclient = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
                if (response.IsSuccessStatusCode)
                {
                    string strResponse = response.Content.ToString();
                    NotificationModel _NotifiModel = JsonConvert.DeserializeObject<NotificationModel>(strResponse);
                    Notification = _NotifiModel;
                    NotificationShowAync?.Invoke();
                }
                else
                {
                    throw new Exception();
                }
                httpclient.Dispose();
            }
            catch
            {
                Notification = new NotificationModel();
            }
        }

        public async Task ResetNotification(string type)
        {
            try
            {
                string posturi = API.REMIND_UNREAD_SET_COUNT;
                
                HttpClient httpClient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(posturi));
                HttpFormUrlEncodedContent postData = new HttpFormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("source", "211160679"),
                        new KeyValuePair<string, string>("access_token", App.WeicoAccessToken),
                        new KeyValuePair<string, string>("type", type)

                    }
                );
                request.Content = postData;
                HttpResponseMessage response = await httpClient.SendRequestAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    if(type == "cmt" || type == "mention_status")
                    {
                        MessageDisable?.Invoke();
                    }
                    else if(type == "follower")
                    {
                        FollowerDisable?.Invoke();
                    }
                }
                
            }
            catch
            {
            }
        }

        public event Action NotificationShowAync;
        public event Action FollowerDisable;
        public event Action MessageDisable;

        private bool IsRefreshing;
        private string _launcherArg;
        public bool IsFirstActived { get; set; } = true;
    }
}
