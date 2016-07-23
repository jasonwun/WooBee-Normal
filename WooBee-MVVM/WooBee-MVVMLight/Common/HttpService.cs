using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Web.Http;

namespace WooBee_MVVMLight
{
    public class HttpService
    {

        public static async Task<HttpResponseMessage>  GetData(int PageIndex, CancellationToken token, string ApiUrl)
        {
            string Uri = ApiUrl;
            Uri += "?source=";
            Uri += App.AppKey;
            Uri += "&access_token=";
            Uri += App.WeicoAccessToken;
            Uri += "&page=";
            Uri += PageIndex.ToString();
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            return response;
        }
    }
}
