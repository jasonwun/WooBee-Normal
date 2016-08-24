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
    public class UserWeiboDataViewModel : DataViewModelBase<Weibo>
    {
        private string param;
        private string posttype;

        public UserWeiboDataViewModel(string Param, string PostType)
        {
            param = Param;
            posttype = PostType;
        }

        protected override void ClickItem(Weibo item)
        {
            throw new NotImplementedException();
        }

        protected override async Task<IEnumerable<Weibo>> GetList(int pageIndex)
        {
            try
            {
                string Uri = API.USER_TIMELINE;
                Uri += "?access_token=";
                Uri += App.WeicoAccessToken;
                if (posttype == "uid")
                    Uri += "&uid=";
                else if (posttype == "screen_name")
                    Uri += "&screen_name=";
                Uri += param;
                //Uri += "&gsid=_2A256selmDeTxGedO41AY9CjKyT2IHXVX53uurDV6PUJbjdANLUvekWpFzGTIoZzbkdNPhNil6IFvjFXIcw..";
                HttpClient httpclient = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
                if (response.IsSuccessStatusCode)
                {
                    string strResponse = response.Content.ToString();
                    WeiboModel _weiboModel = JsonConvert.DeserializeObject<WeiboModel>(strResponse);
                    return _weiboModel.Statuses;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                return new List<Weibo>();
            }
        }

        protected override void LoadMoreItemCompleted(IEnumerable<Weibo> list, int index)
        {
        }
    }
}
