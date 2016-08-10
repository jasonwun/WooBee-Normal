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

namespace WooBee_MVVMLight
{
    public class WeiboDetailCommentRepostData:ViewModelBase
    {
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
        public WeiboDetailCommentRepostData() { }


        public async Task Get(string a)
        {
            await GetComment(a);
            await GetRepsot(a);
        }

        private async Task GetRepsot(string a)
        {
            string Uri = "https://api.weibo.com/2/statuses/repost_timeline.json?access_token=";
            Uri += App.WeicoAccessToken;
            Uri += "&id=";
            Uri += a;
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            RepostModel repostUti = JsonConvert.DeserializeObject<RepostModel>(strResponse);
            RepostList = repostUti.Repost;
        }

        private async Task GetComment(string a)
        {
            string Uri = "https://api.weibo.com/2/comments/show.json?access_token=";
            Uri += App.WeicoAccessToken;
            Uri += "&id=";
            Uri += a;
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            CommentModel commentuti = JsonConvert.DeserializeObject<CommentModel>(strResponse);
            CommentList = commentuti.Comments;
        }
    }
}
