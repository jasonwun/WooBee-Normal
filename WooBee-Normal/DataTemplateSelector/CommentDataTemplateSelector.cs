using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WooBee_Normal
{
    public class CommentDataTemplateSelector : DataTemplateSelector
    {
        //Datatemplate with replied to my weibo
        public DataTemplate RepliedToWeiboTemplate { get; set; }

        //Datatemplate with replied to my comments
        public DataTemplate RepliedToCommentTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var listItem = item as Comment;
            if (listItem.ReplyComment != null)
                return RepliedToCommentTemplate;
            else
                return RepliedToWeiboTemplate;

            return base.SelectTemplateCore(item, container);
        }
    }
}
