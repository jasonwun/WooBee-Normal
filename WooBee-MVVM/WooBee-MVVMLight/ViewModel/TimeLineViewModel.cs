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
                return _goToUserViewCommand = new RelayCommand(() =>
                {
                    NavigationService.NaivgateToPage(typeof(UserView),App.Uid);
                });
            }
        }

        private RelayCommand _goToMessageViewCommand;
        public RelayCommand GoToMessageViewCommand
        {
            get
            {
                if (_goToMessageViewCommand != null) return _goToMessageViewCommand;
                return _goToMessageViewCommand = new RelayCommand(() =>
                {
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
                    NavigationService.NaivgateToPage(typeof(NewPostView));
                });
            }
        }


        public TimeLineViewModel()
        {
            MainList = new ObservableCollection<Weibo>();
            TimeLineDataViewModel = new TimeLineDataViewModel();
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
                await RefreshAsync();
            }
        }

        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            await TimeLineDataViewModel.RefreshAsync();
            MainList = TimeLineDataViewModel.DataList;
            IsRefreshing = false;

        }

        
        private bool IsRefreshing;
        public bool IsFirstActived { get; set; } = true;
    }
}
