using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace WooBee_Normal
{
    public sealed class MyRichTextBlock : Control
    {
        RichTextBlock _richTextBlock;
        StringBuilder builder = new StringBuilder();


        public MyRichTextBlock()
        {
            this.DefaultStyleKey = typeof(MyRichTextBlock);

            foreach (var key in emojiDict.Keys)
            {
                builder.Append(key.Replace("[", @"\[").Replace("]", @"\]"));//将字典中的[ 和 ] 符号转换成\]和\[,因为在正则表达式中[
                                                                            // 和 ] 符号有特殊含义。
                builder.Append("|");
            }
            builder.Remove(builder.Length - 1, 1); //去掉最后一个“|”符号，否则匹配时会多出一个""字符。
        }



        private readonly Dictionary<string, string> emojiDict = new Dictionary<string, string>
    {
        {"[哈哈]", "[哈哈]"},
        {"[嘻嘻]", "[嘻嘻]"},
        {"[微笑]", "[微笑]"},
        {"[爱你]", "[爱你]"},
            {"🎅","🎅"},
            {"[doge]","[doge]"},
            {"[good]","[good]"},
            {"[haha]","[haha]"},
            {"[hold住]","[hold住]"},
            {"[NO]","[NO]"},
            {"[ok]","[ok]"},
            {"[下雨]","[下雨]"},
            {"[不好意思]","[不好意思]"},
            {"[不想上班]","[不想上班]"},
            {"[丘比特]","[丘比特]"},
            {"[互相膜拜]","[互相膜拜]"},
            {"[互粉]","[互粉]"},
            {"[亲亲]","[亲亲]"},
            {"[伤心]","[伤心]" },
            {"[作揖]","[作揖]" },
            {"[偷乐]","[偷乐]" },
            {"[偷笑]","[偷笑]" },
            {"[兔子]","[兔子]" },
            {"[别烦我]","[别烦我]" },
            {"[可怜]","[可怜]" },
            {"[可爱]","[可爱]" },
            {"[右哼哼]","[右哼哼]" },
            {"[吃惊]","[吃惊]" },
            {"[吐]","[吐]" },
            {"[哈欠]","[哈欠]" },
            {"[哼]","[哼]" },
            {"[喜]","[喜]" },
            {"[嘘]","[嘘]" },
            {"[噢耶]","[噢耶]" },
            {"[囧]","[囧]" },
            {"[困]","[困]" },
            {"[困死了]","[困死了]" },
            {"[围脖]","[围脖]" },
            {"[围观]","[围观]" },
            {"[太开心]","[太开心]" },
            {"[太阳]","[太阳]" },
            {"[失望]","[失望]" },
            {"[奥特曼]","[奥特曼]" },
            {"[女孩儿]","[女孩儿]" },
            {"[好喜欢]","[好喜欢]" },
            {"[好囧]","[好囧]" },
            {"[好棒]","[好棒]" },
            {"[好爱哦]","[好爱哦]" },
            {"[委屈]","[委屈]" },
            {"[威武]","[威武]" },
            {"[害羞]","[害羞]" },
            {"[崩溃]","[崩溃]" },
            {"[左哼哼]","[左哼哼]" },
            {"[巨汗]","[巨汗]" },
            {"[干杯]","[干杯]" },
            {"[弱]","[弱]" },
            {"[得意地笑]","[得意地笑]" },
            {"[微风]","[微风]" },
            {"[心]","[心]" },
            {"[怒]","[怒]" },
            {"[怒骂]","[怒骂]" },
            {"[思考]","[思考]" },
            {"[悲伤]","[悲伤]" },
            {"[悲催]","[悲催]" },
            {"[想一想]","[想一想]" },
            {"[感冒]","[感冒]" },
            {"[抓狂]","[抓狂]" },
            {"[抠鼻屎]","[抠鼻屎]" },
            {"[拜拜]","[拜拜]" },
            {"[拳头]","[拳头]" },
            {"[挖鼻]","[挖鼻]" },
            {"[挤眼]","[挤眼]" },
            {"[推荐]","[推荐]" },
            {"[握手]","[握手]" },
            {"[晕]","[晕]" },
            {"[月亮]","[月亮]" },
            {"[有鸭梨]","[有鸭梨]" },
            {"[来]","[来]" },
            {"[杰克逊]","[杰克逊]" },
            {"[求关注]","[求关注]" },
            {"[汗]","[汗]" },
            {"[泪]","[泪]" },
            {"[泪流满面]","[泪流满面]" },
            {"[浪]","[浪]" },
            {"[浮云]","[浮云]" },
            {"[照相机]","[照相机]" },
            {"[熊猫]","[熊猫]" },
            {"[猪头]","[猪头]" },
            {"[玫瑰]","[玫瑰]" },
            {"[生病]","[生病]" },
            {"[甩甩手]","[甩甩手]" },
            {"[男孩儿]","[男孩儿]" },
            {"[疑问]","[疑问]" },
            {"[白眼]","[白眼]" },
            {"[睡]","[睡]" },
            {"[瞧瞧]","[瞧瞧]" },
            {"[礼物]","[礼物]" },
            {"[神马]","[神马]" },
            {"[笑cry]","[笑cry]" },
            {"[笑哈哈]","[笑哈哈]" },
            {"[织]","[织]" },
            {"[给力]","[给力]" },
            {"[给劲]","[给劲]" },
            {"[绿丝带]","[绿丝带]" },
            {"[羞嗒嗒]","[羞嗒嗒]" },
            {"[群体围观]","[群体围观]" },
            {"[耶]","[耶]" },
            {"[肥皂]","[肥皂]" },
            {"[色]","[色]" },
            {"[草泥马]","[草泥马]" },
            {"[萌]","[萌]" },
            {"[蛋糕]","[蛋糕]" },
            {"[蜡烛]","[蜡烛]" },
            {"[衰]","[衰]" },
            {"[被电]","[被电]" },
            {"[许愿]","[许愿]" },
            {"[话筒]","[话筒]" },
            {"[赞]","[赞]" },
            {"[赞啊]","[赞啊]" },
            {"[躁狂症]","[躁狂症]" },
            {"[转发]","[转发]" },
            {"[鄙视]","[鄙视]" },
            {"[酷]","[酷]" },
            {"[钟]","[钟]" },
            {"[钱]","[钱]" },
            {"[闭嘴]","[闭嘴]" },
            {"[阴险]","[阴险]" },
            {"[雷锋]","[雷锋]" },
            {"[震惊]","[震惊]" },
            {"[音乐]","[音乐]" },
            {"[顶]","[顶]" },
            {"[飞机]","[飞机]" },
            {"[馋嘴]","[馋嘴]" },
            {"[鲜花]","[鲜花]" },
            {"[黑线]","[黑线]" },
            {"[鼓掌]","[鼓掌]" },
            {"☀","☀" },
            {"☁","☁" },
            {"✌","✌" },
            {"❤","❤" },
            {"✊","✊" },
            {"☔","☔" },
            {"☕","☕" },
            {"⚡","⚡" },
            {"🌙","🌙" },
            {"🌟","🌟" },
            {"🌹","🌹" },
            {"🌻","🌻" },
            {"🍁","🍁" },
            {"🍃","🍃" },
            {"🍺","🍺" },
            {"🎀","🎀" },
            {"🎁","🎁" },
            {"🎂","🎂" },
            {"🏠","🏠" },
            {"🐱","🐱" },
            {"🐶","🐶" },
            {"👄","👄" },
            {"👆","👆" },
            {"👇","👇" },
            {"👈","👈" },
            {"👉","👉" },
            {"👊","👊" },
            {"👌","👌" },
            {"👍","👍" },
            {"👎","👎" },
            {"👏","👏" },
            {"👗","👗" },
            {"👦","👦" },
            {"👧","👧" },
            {"👨","👨" },
            {"👩","👩" },
            {"👻","👻" },
            {"👿","👿" },
            {"💔","💔" },
            {"💣","💣" },
            {"💪","💪" },
            {"📱","📱" },
            {"🔍","🔍" },
            {"🕙","🕙" },
            {"😁","😁" },
            {"😂","😂" },
            {"😃","😃" },
            {"😄","😄" },
            {"😉","😉" },
            {"😊","😊" },
            {"😌","😌" },
            {"😍","😍" },
            {"😏","😏" },
            {"😒","😒" },
            {"😓","😓" },
            {"😔","😔" },
            {"😖","😖" },
            {"😘","😘" },
            {"😚","😚" },
            {"😜","😜" },
             {"😝","😝" },
            {"😞","😞" },
            {"😠","😠" },
            {"😡","😡" },
            {"😢","😢" },
             {"😣","😣" },
            {"😥","😥" },
            {"😨","😨" },
            {"😪","😪" },
            {"😭","😭" },
             {"😰","😰" },
            {"😱","😱" },
            {"😲","😲" },
            {"😳","😳" },
            {"😷","😷" },
             {"🙏","🙏" },
            {"🚗","🚗" },
            {"[喵喵]","[喵喵]" },
            {"[最右]","[最右]" },
    };

        protected override void OnApplyTemplate()
        {
            _richTextBlock = GetTemplateChild("ChildRichTextBlock") as RichTextBlock;
            //SetRichTextBlock(Text);
        }


        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(String), typeof(MyRichTextBlock), new PropertyMetadata("", OnTextChange));

        private static void OnTextChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rtb = (MyRichTextBlock)d;
            rtb.SetRichTextBlock(e.NewValue.ToString());
        }

        private void SetRichTextBlock(string value)
        {
            
            Regex urlRx = new Regex(@"(?<url>(http:[/][/]|www.)([a-z]|[A-Z]|[0-9]|[/.]|[~])*)", RegexOptions.IgnoreCase);
            if (_richTextBlock == null)
                return;
            
            MatchCollection matches = urlRx.Matches(value);
            var r = new Regex(builder.ToString());
            var mc = r.Matches(value);
            foreach (Match m in mc)
            {
                value = value.Replace(m.Value, string.Format(@"<InlineUIContainer><Border><Image Source=""ms-appx:///Assets/Emoji/{0}.png"" Margin=""2,0,2,0"" Width=""30"" Height=""30""/></Border></InlineUIContainer> ", emojiDict[m.Value]));
            }
            foreach (Match match in matches)
            {
                string url = match.Groups["url"].Value;
                value = value.Replace(url,
                    string.Format(@"<InlineUIContainer><Border><HyperlinkButton Margin=""0,0,0,-4"" Padding=""0,2,0,0"" NavigateUri =""{0}""><StackPanel HorizontalAlignment=""Center"" Height=""25"" Width=""90"" Background=""#FFB8E9FF"" Orientation = ""Horizontal"">
                        <Image Margin=""5,0,0,0"" Source = ""ms-appx:///Assets/Link.png"" Width = ""15"" Height = ""15""/><TextBlock Margin=""4,2.5,0,0"" Text=""网页链接"" Foreground=""White"" FontFamily=""Microsoft YaHei UI"" FontSize=""14"" FontWeight=""Bold""/>
                    </StackPanel>
                </HyperlinkButton>
            </Border>
        </InlineUIContainer>", url));
            }
            //value = value.Replace("\r\n", "<LineBreak/>"); //将换行符转换成<LineBreak/>,用于实现换行。


            var xaml = string.Format(@"<Paragraph 
                                        xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                        xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                                    <Paragraph.Inlines>
                                    <Run></Run>
                                      {0}
                                    </Paragraph.Inlines>
                                </Paragraph>", value);
            var p = (Paragraph)XamlReader.Load(xaml);
            _richTextBlock.Blocks.Add(p);
        }
    }
}
