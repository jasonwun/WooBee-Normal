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
            {"[最右]","01" },{"[微笑]","02" },{"[嘻嘻]","03" },{"[哈哈]","04" },{"[爱你]","05" },{"[挖鼻]","06" },{"[吃惊]","07" },
            {"[晕]","08" },{"[泪]","09" },{"[馋嘴]","10" },{"[抓狂]","11" },{"[哼]","12" },{"[可爱]","13" },{"[怒]","14" },
            {"[汗]","15" },{"[害羞]","16" },{"[睡]","17" },{"[钱]","18" },{"[偷笑]","19" },{"[笑cry]","20" },{"[doge]","21" },{"[喵喵]","22" },
            {"[酷]","23" },{"[衰]","24" },{"[闭嘴]","25" },{"[鄙视]","26" },{"[色]","27" },{"[鼓掌]","28" },{"[悲伤]","29" },
            {"[思考]","30" },{"[生病]","31" },{"[亲亲]","32" },{"[怒骂]","33" },{"[太开心]","34" },{"[白眼]","35" },{"[右哼哼]","36" },
            {"[左哼哼]","37" },{"[嘘]","38" },{"[委屈]","39" },
            {"[吐]","40" },
            {"[可怜]","41" },
            {"[哈欠]","42" },
            {"[挤眼]","43" },
            {"[失望]","44" },
            {"[顶]","45" },
            {"[疑问]","46" },
            {"[困]","47" },
            {"[感冒]","48" },
            {"[拜拜]","49" },
            {"[黑线]","50" },
            {"[阴险]","51" },
            {"[互粉]","52" },
            {"[心]","53" },
            {"[伤心]","54" },
            {"[猪头]","55" },
            {"[熊猫]","56" },
            {"[兔子]","57" },
            {"[握手]","58" },
            {"[作揖]","59" },
            {"[赞]","60" },
            {"[耶]","61" },
            {"[good]","62" },
            {"[弱]","63" },
            {"[NO]","64" },
            {"[ok]","65" },
            {"[haha]","66" },
            {"[来]","67" },
            {"[拳头]","68" },
            {"[威武]","69" },
            {"[鲜花]","70" },
            {"[钟]","71" },
            {"[浮云]","72" },
            {"[飞机]","73" },
            {"[月亮]","74" },
            {"[太阳]","75" },
            {"[微风]","76" },
            {"[下雨]","77" },
            {"[给力]","78" },
            {"[神马]","79" },
            {"[围观]","80" },
            {"[话筒]","81" },
            {"[奥特曼]","82" },
            {"[草泥马]","83" },
            {"[萌]","84" },
            {"[囧]","85" },
            {"[织]","86" },
            {"[礼物]","87" },
            {"[喜]","88" },
            {"[围脖]","89" },
            {"[音乐]","90" },
            {"[绿丝带]","91" },
            {"[蛋糕]","92" },
            {"[蜡烛]","93" },
            {"[干杯]","94" },
            {"[男孩儿]","95" },
            {"[女孩儿]","96" },
            {"[肥皂]","97" },
            {"[照相机]","98" },
            {"[浪]","99" },
            {"😃","100" },
            {"😍","101" },
            {"😒","102" },
            {"😳","103" },
            {"😁","104" },
            {"😘","105" },
            {"😉","106" },
            {"😠","107" },
            {"😞","108" },
            {"😥","109" },
            {"😭","110" },
            {"😝","111" },
            {"😡","112" },
            {"😣","113" },
            {"😔","114" },
            {"😄","115" },
            {"😷","116" },
            {"😚","117" },
            {"😓","118" },
            {"😂","119" },
            {"😊","120" },
            {"😢","121" },
            {"😜","122" },
            {"😨","123" },
            {"😰","124" },
            {"😲","125" },
            {"😏","126" },
            {"😱","127" },
            {"😪","128" },
            {"😖","129" },
            {"😌","130" },
            {"👿","131" },
            {"👻","132" },
            {"🎅","133" },
            {"👧","134" },
            {"👦","135" },
            {"👩","136" },
            {"👨","137" },
            {"🐶","138" },
            {"🐱","139" },
            {"👍","140" },
            {"👎","141" },
            {"👊","142" },
            {"✊","143" },
            {"✌","144" },
            {"💪","145" },
            {"👏","146" },
            {"👈","147" },
            {"👆","148" },
            {"👉","149" },
            {"👇","150" },
            {"👌","151" },
            {"❤","152" },
            {"💔","153" },
            {"🙏","154" },
            {"☀","155" },
            {"🌙","156" },
            {"🌟","157" },
            {"⚡","158" },
            {"☁","159" },
            {"☔","160" },
            {"🍁","161" },
            {"🌻","162" },
            {"🍃","163" },
            {"👗","164" },
            {"🎀","165" },
            {"👄","166" },
            {"🌹","167" },
            {"☕","168" },
            {"🎂","169" },
            {"🕙","170" },
            {"🍺","171" },
            {"🔍","172" },
            {"📱","173" },
            {"🏠","174" },
            {"🚗","175" },
            {"🎁","176" },
            {"💣","177" },
            {"⚽","178" },
            {"[笑哈哈]","179" },
            {"[好爱哦]","180" },
            {"[噢耶]","181" },
            {"[偷乐]","182" },
            {"[泪流满面]","183" },
            {"[巨汗]","184" },
            {"[抠鼻屎]","185" },
            {"[求关注]","186" },
            {"[好喜欢]","187" },
            {"[崩溃]","188" },
            {"[好囧]","189" },
            {"[震惊]","190" },
            {"[别烦我]","191" },
            {"[不好意思]","192" },
            {"[羞嗒嗒]","193" },
            {"[得意地笑]","194" },
            {"[纠结]","195" },
            {"[给劲]","196" },
            {"[悲催]","197" },
            {"[甩甩手]","198" },
            {"[好棒]","199" },
            {"[瞧瞧]","200" },
            {"[不想上班]","201" },
            {"[困死了]","202" },
            {"[许愿]","203" },
            {"[丘比特]","204" },
            {"[有鸭梨]","205" },
            {"[想一想]","206" },
            {"[躁狂症]","207" },
            {"[转发]","208" },
            {"[互相膜拜]","209" },
            {"[雷锋]","210" },
            {"[杰克逊]","211" },
            {"[玫瑰]","212" },
            {"[hold住]","213" },
            {"[群体围观]","214" },
            {"[推荐]","215" },
            {"[赞啊]","216" },
            {"[被电]","217" },
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
