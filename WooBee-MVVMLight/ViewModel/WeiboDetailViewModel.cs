using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooBee_MVVM.Model;
using WooBee_MVVMLight.Common;

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

        private RelayCommand _commentCommand;
        public RelayCommand CommentCommand
        {
            get
            {
                if (_commentCommand != null) return _commentCommand;
                return _commentCommand = new RelayCommand(() =>
                {
                    PostWeiboNaviParamContainer postNaviContainer = new PostWeiboNaviParamContainer();
                    postNaviContainer.PostType = "comment";
                    postNaviContainer.WeiboID = Weibo.ID;
                    NavigationService.NaivgateToPage(typeof(NewPostView), postNaviContainer);
                });
            }
        }

        private RelayCommand _repostCommand;
        public RelayCommand RepostCommand
        {
            get
            {
                if (_repostCommand != null) return _repostCommand;
                return _repostCommand = new RelayCommand(() =>
                {
                    PostWeiboNaviParamContainer postNaviContainer = new PostWeiboNaviParamContainer();
                    postNaviContainer.PostType = "repost";
                    postNaviContainer.WeiboID = Weibo.ID;
                    postNaviContainer.RepostText = Weibo.Text;
                    postNaviContainer.RepostScreenName = Weibo.User.ScreenName;
                    NavigationService.NaivgateToPage(typeof(NewPostView), postNaviContainer);
                });
            }
        }

        private RelayCommand _likeCommand;
        public RelayCommand LikeCommand
        {
            get
            {
                if (_likeCommand != null) return _likeCommand;
                return _likeCommand = new RelayCommand(() =>
                {
                    //TODO
                });
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
