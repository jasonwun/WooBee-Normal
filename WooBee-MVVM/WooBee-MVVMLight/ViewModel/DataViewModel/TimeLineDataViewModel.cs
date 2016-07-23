using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooBee_MVVM.Model;
using WooBee_MVVMLight.Common;

namespace WooBee_MVVMLight
{
    public class TimeLineDataViewModel : DataViewModelBase<Weibo>
    {
        protected override void ClickItem(Weibo item)
        {
            throw new NotImplementedException();
        }

        protected override async Task<IEnumerable<Weibo>> GetList(int pageIndex)
        {
            try
            {
                var result = await HttpService.GetData(pageIndex, CTSFactory.MakeCTS(2000).Token, API.HOME_TIMELINE);
                if (result.IsSuccessStatusCode)
                {
                    string strResponse = result.Content.ToString();
                    WeiboModel _weiboModel = JsonConvert.DeserializeObject<WeiboModel>(strResponse);
                    return _weiboModel.Statuses;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch(Exception e)
            {
                return new List<Weibo>();
            }
        }

        protected override void LoadMoreItemCompleted(IEnumerable<Weibo> list, int index)
        {
            
        }
    }
}
