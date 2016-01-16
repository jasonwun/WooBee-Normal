using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Web.Http;

namespace WooBee_Normal
{
    class OauthSina
    {
        public const string RedirectUri = "https://api.weibo.com/oauth2/default.html";

        private string Code { get; set; }

        private string access_token { get; set; }

        private string expires_in { get; set; }

        public void SetCode(string code)
        {
            Code = code;
        }

        public ApplicationDataContainer localsetting = ApplicationData.Current.LocalSettings;

        public async Task GetAccessToekn()
        {
            HttpClient httpclient = new HttpClient();
            string posturi = "https://api.weibo.com/oauth2/access_token?";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(posturi));
            HttpFormUrlEncodedContent postData = new HttpFormUrlEncodedContent(
                new List<KeyValuePair<string, string>>
                {
                        new KeyValuePair<string, string>("client_id", App.client_id),
                        new KeyValuePair<string, string>("client_secret", App.client_secret),
                        new KeyValuePair<string, string>("grant_type","authorization_code"),
                        new KeyValuePair<string, string>("redirect_uri", RedirectUri),
                        new KeyValuePair<string, string>("code",Code),

                }
            );
            request.Content = postData;
            HttpResponseMessage response = await httpclient.SendRequestAsync(request);
            string responseString = await response.Content.ReadAsStringAsync();
            JsonObject token = JsonObject.Parse(responseString);
            App.access_token = token.GetNamedString("access_token").ToString();
            App.localsetting.Values["access_token"] = token.GetNamedString("access_token").ToString();
        }


    }
}
