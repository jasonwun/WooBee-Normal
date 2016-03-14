using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class CommentSource : ObservableCollection<Comment>, ISupportIncrementalLoading
    {
        public int lastItem = 0;
        public static int count = 0;
        public static int commentpage_num = 1;
        CommentUti commentuti = new CommentUti();


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

                List<Comment> items = new List<Comment>();
                await GetComment();
                await coreDispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    foreach (Comment item in commentuti.Comment)
                    {
                        if (item.ReplyComment != null && item.ReplyComment.Text.Length > 22)
                            item.ReplyComment.Text = item.ReplyComment.Text.Substring(0, 22);
                        else if(item.Status!= null && item.Status.Text.Length > 22)
                            item.Status.Text = item.Status.Text.Substring(0, 22);
                        this.Add(item);
                        lastItem++;
                    }
                });
                return new LoadMoreItemsResult() { Count = count };
            }).AsAsyncOperation<LoadMoreItemsResult>();
        }

        public async Task GetComment()
        {
            string Uri = "https://api.weibo.com/2/comments/to_me.json?source=";
            Uri += App.client_id;
            Uri += "&access_token=";
            Uri += App.access_token;
            Uri += "&page=";
            Uri += commentpage_num.ToString();
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpclient.GetAsync(new Uri(Uri, UriKind.Absolute));
            string strResponse = response.Content.ToString();
            commentuti = JsonConvert.DeserializeObject<CommentUti>(strResponse);
            if (count == 1)
                count++;
            else
            {
                commentpage_num++;
            }
        }

    }
}
