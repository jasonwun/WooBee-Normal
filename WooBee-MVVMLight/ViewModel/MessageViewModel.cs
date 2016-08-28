using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooBee_MVVM.Model;

namespace WooBee_MVVMLight
{
    public class MessageViewModel : ViewModelBase, INavigable
    {
        private CommentDataViewModel _commentDataViewModel;
        public CommentDataViewModel CommentDataViewModel
        {
            get
            {
                return _commentDataViewModel;
            }
            set
            {
                if(_commentDataViewModel != value)
                {
                    _commentDataViewModel = value;
                    RaisePropertyChanged(() => CommentDataViewModel);
                }
            }
        }

        private MentionDataViewModel _mentionDataViewModel;
        public MentionDataViewModel MentionDataViewModel
        {
            get
            {
                return _mentionDataViewModel;
            }
            set
            {
                if(_mentionDataViewModel != value)
                {
                    _mentionDataViewModel = value;
                    RaisePropertyChanged(() => MentionDataViewModel);
                }
            }
        }

        private ObservableCollection<Comment> _commentList;
        public ObservableCollection<Comment> CommentList
        {
            get
            {
                return _commentList;
            }
            set
            {
                if (_commentList != value)
                {
                    _commentList = value;
                    RaisePropertyChanged(() => CommentList);
                }
            }
        }

        private ObservableCollection<Weibo> _mentionList;
        public ObservableCollection<Weibo> MentionList
        {
            get
            {
                return _mentionList;
            }
            set
            {
                if(_mentionList != value)
                {
                    _mentionList = value;
                    RaisePropertyChanged(() => MentionList);
                }
            }
        }

        

        public MessageViewModel()
        {
            CommentList = new ObservableCollection<Comment>();
            CommentDataViewModel = new CommentDataViewModel();
            MentionDataViewModel = new MentionDataViewModel();
            
        }


        public void Activate(object parameter) { }
        public void Deactivate(object parameter) { }

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
            await CommentDataViewModel.RefreshAsync();
            CommentList = CommentDataViewModel.DataList;
            await MentionDataViewModel.RefreshAsync();
            MentionList = MentionDataViewModel.DataList;
            IsRefreshing = false;
        }

        private bool IsRefreshing;
        public bool IsFirstActived { get; set; } = true;

    }
}
