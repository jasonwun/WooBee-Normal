using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WooBee_Normal
{
    public class TimelineDataTemplateSelector : DataTemplateSelector
    {
        //normal datatemplat with text
        public DataTemplate NormalTemplate { get; set; }

        //datatemplate with text repost
        public DataTemplate TextRepostTemplate { get; set; }

        //datatemplate with one image
        public DataTemplate OneImageTemplate { get; set; }

        //datatemplate with more than one images
        public DataTemplate MultiImagesTemplate { get; set; }

        //datatemplate with repost and one image
        public DataTemplate RepostOneImageTemplate { get; set;}

        //datatemplate with repost and more than one images
        public DataTemplate RepostMultiImagesTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var listItem = item as Weibo;
            if (listItem.RepostWeibo != null)
            {
                if (listItem.RepostWeibo.PicUrls == null || listItem.RepostWeibo.PicUrls.Count == 0)
                    return TextRepostTemplate;
                else if (listItem.RepostWeibo.PicUrls.Count == 1)
                    return RepostOneImageTemplate;
                else if (listItem.RepostWeibo.PicUrls.Count > 1)
                    return RepostMultiImagesTemplate;

            }
            else if (listItem.RepostWeibo == null)
            {
                if (listItem.PicUrls.Count == 0)
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
