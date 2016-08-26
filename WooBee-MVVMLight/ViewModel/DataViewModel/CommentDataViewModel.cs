using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using WooBee_MVVM.Model;
using WooBee_MVVMLight.Common;

namespace WooBee_MVVMLight
{
    public class CommentDataViewModel : DataViewModelBase<Comment>
    {
        protected override void ClickItem(Comment item)
        {
            throw new NotImplementedException();
        }

        protected override async Task<IEnumerable<Comment>> GetList(int pageIndex)
        {
            try
            {
                string Uri = API.COMMENTS_TO_ME;
                Uri += "?source=";
                Uri += App.AppKey;
                Uri += "&access_token=";
                Uri += App.WeicoAccessToken;
                Uri += "&page=";
                Uri += pageIndex.ToString();
                HttpClient httpclient = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
                if (response.IsSuccessStatusCode)
                {
                    string strResponse = response.Content.ToString();
                    CommentModel _commentModel = JsonConvert.DeserializeObject<CommentModel>(strResponse);
                    return _commentModel.Comments;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                return new List<Comment>();
            }
        }

        protected override void LoadMoreItemCompleted(IEnumerable<Comment> list, int index) { }
    }
}
