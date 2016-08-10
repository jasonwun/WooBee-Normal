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
    public class WeiboDetailViewModel: ViewModelBase, INavigable
    {
        private Weibo _weibo;
        public Weibo Weibo
        {
            get
            {
                return _weibo;
            }
            set
            {
                if(_weibo != value)
                {
                    _weibo = value;
                    RaisePropertyChanged(() => Weibo);
                }
            }
        }

        private WeiboDetailCommentRepostData _weiboDetailCommentRepostData;
        public WeiboDetailCommentRepostData WeiboDetailCommentRepostData
        {
            get
            {
                return _weiboDetailCommentRepostData;
            }
            set
            {
                if(_weiboDetailCommentRepostData != value)
                {
                    _weiboDetailCommentRepostData = value;
                    RaisePropertyChanged(() => WeiboDetailCommentRepostData);
                }
            }
        }

        private ObservableCollection<Weibo> _repostList;
        public ObservableCollection<Weibo> RepostList
        {
            get
            {
                return _repostList;
            }
            set
            {
                if(_repostList != value)
                {
                    _repostList = value;
                    RaisePropertyChanged(() => RepostList);
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
                if(_commentList != value)
                {
                    _commentList = value;
                    RaisePropertyChanged(() => CommentList);
                }
            }
        }


        public WeiboDetailViewModel()
        {
            WeiboDetailCommentRepostData = new WeiboDetailCommentRepostData();
            CommentList = new ObservableCollection<Comment>();
            RepostList = new ObservableCollection<Weibo>();
        }

        public void Activate(object parameter) { }

        public void Deactivate(object parameter) { }

        public async void OnLoaded()
        {
            await WeiboDetailCommentRepostData.Get(Weibo.ID);
            CommentList = WeiboDetailCommentRepostData.CommentList;
            RepostList = WeiboDetailCommentRepostData.RepostList;
        }
    }
}
