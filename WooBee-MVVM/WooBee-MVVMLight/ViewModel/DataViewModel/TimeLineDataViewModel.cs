using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooBee_MVVM.Model;

namespace WooBee_MVVMLight
{
    public class TimeLineDataViewModel : DataViewModelBase<Weibo>
    {
        protected override void ClickItem(Weibo item)
        {
            
        }

        protected override Task<ObservableCollection<Weibo>> GetList(int pageIndex)
        {
            try
            {
                var result = await CloudService.GetImages(pageIndex, (int)DEFAULT_PER_PAGE, CTSFactory.MakeCTS(2000).Token);
                if (result.IsSuccessful)
                {
                    var list = UnsplashImage.ParseListFromJson(result.JsonSrc);
                    return list;
                }
                else
                {
                    //throw new APIException();
                }
            }
            catch(Exception e)
            {
                return new ObservableCollection<Weibo>();
            }
        }

        protected override void LoadMoreItemCompleted(IEnumerable<Weibo> list, int index)
        {
            throw new NotImplementedException();
        }
    }
}
