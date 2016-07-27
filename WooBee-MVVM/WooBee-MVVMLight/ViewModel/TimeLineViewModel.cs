using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WooBee_MVVM.Model;
using System.ComponentModel;

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

            //await SaveMainListDataAsync();
            //await UpdateLiveTileAsync();
        }

        
        private bool IsRefreshing;
        public bool IsFirstActived { get; set; } = true;
    }
}
