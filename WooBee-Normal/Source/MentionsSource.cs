using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.Web.Http;

namespace WooBee_Normal
{
    public class MentionsSource : ObservableCollection<Weibo>, ISupportIncrementalLoading, INotifyPropertyChanged
    {
        public int lastItem = 0;
        HomeWeibo homeweibo = new HomeWeibo();
        private int _sinceid { get; set; }
        private int _currentid = 89089089;
        private int page_num = 1;
        private int count = 0;
        

        public bool HasMoreItems
        {
            get
            {
                if (lastItem == 50)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            CoreDispatcher coreDispatcher = Window.Current.Dispatcher;

            return Task.Run<LoadMoreItemsResult>(async () =>
            {
                try
                {
                    await GetMentions();
                    if (_currentid == _sinceid)
                    {
                        await coreDispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            foreach (Weibo item in homeweibo.Statuses)
                            {
                                this.Add(item);
                                lastItem++;
                            }
                        });
                    }
                    else
                        _currentid = _sinceid;
                }
                catch (Exception ex)
                {
                    string abc = ex.ToString();
                }



                return new LoadMoreItemsResult() { Count = count };
            }).AsAsyncOperation<LoadMoreItemsResult>();
        }

        private async Task GetMentions()
        {
            string Uri = "https://api.weibo.com/2/statuses/mentions.json?access_token=";
            Uri += App.weico_access_token;
            Uri += "&page=";
            Uri += page_num.ToString();
            Uri += "&since_id=";
            Uri += _sinceid.ToString();
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            homeweibo = JsonConvert.DeserializeObject<HomeWeibo>(strResponse);
            if (count == 0)
                count++;
            else
            {
                page_num++;
            }
        }
    }
}
