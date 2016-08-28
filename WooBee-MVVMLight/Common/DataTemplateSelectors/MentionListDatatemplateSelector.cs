using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WooBee_MVVM.Model;

namespace WooBee_MVVMLight
{
    public class MentionListDatatemplateSelector : DataTemplateSelector
    {
        //normal datatemplat with text
        public DataTemplate NormalTemplate { get; set; }

        //datatemplate with one image
        public DataTemplate OneImageTemplate { get; set; }

        //datatemplate with more than one images
        public DataTemplate MultiImagesTemplate { get; set; }

        //datatemplate with repost
        public DataTemplate MentionToMe { get; set; }


        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var listItem = item as Weibo;
            if (listItem.RepostWeibo != null)
            {
                return MentionToMe;
            }
            else
            {
                if (listItem.PicUrls == null || listItem.PicUrls.Count == 0)
                {
                    return NormalTemplate;
                }
                else if (listItem.PicUrls.Count == 1)
                    return OneImageTemplate;
                else if (listItem.PicUrls.Count > 1)
                    return MultiImagesTemplate;
            }
            

            return base.SelectTemplateCore(item, container);
        }
    }
}
