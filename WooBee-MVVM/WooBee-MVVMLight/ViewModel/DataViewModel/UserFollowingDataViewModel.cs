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
    public class UserFollowingDataViewModel: DataViewModelBase<User>
    {
        private string iD;

        public UserFollowingDataViewModel(string iD)
        {
            this.iD = iD;
        }

        protected override void ClickItem(User item)
        {
            throw new NotImplementedException();
        }

        protected override async Task<IEnumerable<User>> GetList(int pageIndex)
        {
            try
            {
                string Uri = API.FRIENDSHIPS_FRIENDS;
                Uri += "?access_token=";
                Uri += App.WeicoAccessToken;
                Uri += "&screen_name=";
                Uri += iD;
                HttpClient httpclient = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
                if (response.IsSuccessStatusCode)
                {
                    string strResponse = response.Content.ToString();
                    UserModel _userModel = JsonConvert.DeserializeObject<UserModel>(strResponse);
                    return _userModel.Users;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                return new List<User>();
            }
        }

        protected override void LoadMoreItemCompleted(IEnumerable<User> list, int index)
        {
        }
    }
}
