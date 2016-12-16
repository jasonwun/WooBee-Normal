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
    public class MentionDataViewModel : DataViewModelBase<Weibo>
    {
        protected override void ClickItem(Weibo item)
        {
            throw new NotImplementedException();
        }

        protected override async Task<IEnumerable<Weibo>> GetList(int pageIndex)
        {
            try
            {
                string Uri = API.MENTIONS;
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
                    WeiboModel _weiboModel = JsonConvert.DeserializeObject<WeiboModel>(strResponse);
                    return _weiboModel.Statuses;
                }
                else
                {
                    throw new Exception();
                }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                return new List<Weibo>();
            }
        }

        protected override void LoadMoreItemCompleted(IEnumerable<Weibo> list, int index) { }
    }
}
