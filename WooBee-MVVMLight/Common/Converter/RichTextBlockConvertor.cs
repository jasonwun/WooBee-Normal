﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using WooBee_MVVMLight.Common;

namespace WooBee_MVVMLight
{
    public class RichTextBlockConvertor : DependencyObject 
    {
        static StringBuilder builder = App.builder;
        static Dictionary<string, string> emojiDict = App.emojiDict;

        public static string GetText(DependencyObject obj)
        {
            return (obj.GetValue(TextProperty)).ToString();
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(RichTextBlockConvertor), new PropertyMetadata(String.Empty, OnTextChanged));

        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            StringConversion(sender, e);
        }

        private static async Task StringConversion(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as RichTextBlock;
            try
            {
                if (control != null)
                {
                    control.Blocks.Clear();
                    string value = e.NewValue.ToString();


                    value = value.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");


                    Regex urlRx = new Regex("http(s)?://([a-zA-Z|\\d]+\\.)+[a-zA-Z|\\d]+(/[a-zA-Z|\\d|\\-|\\+|_./?%=]*)?", RegexOptions.IgnoreCase);
                    Regex hashtagRx = new Regex(@"#[^#]+#");
                    Regex usernameRx = new Regex(@"@[^,，：:\s@]+");
                    MatchCollection matches = urlRx.Matches(value);
                    MatchCollection hastagmc = hashtagRx.Matches(value);
                    MatchCollection usermc = usernameRx.Matches(value);
                    string asd = builder.ToString();
                    var r = new Regex(builder.ToString());
                    var mc = r.Matches(value);


                    foreach (Match m in mc)
                    {
                        value = value.Replace(m.Value, string.Format(@"<InlineUIContainer><Border><StackPanel Margin = ""2,0,-3,0"" ><Image Margin=""0,7,0,0"" Source =""/Assets/Emoticon/{0}.png"" Width=""15"" Height=""15"" /></StackPanel></Border></InlineUIContainer> ", emojiDict[m.Value]));
                    }
                    foreach (Match match in matches)
                    {

                        string url = match.Value;
                        if (value.Contains(string.Format(@"全文： {0}...", url)))
                            value = value.Replace(string.Format(@"全文： {0}...", url), "");
                        if (value.Contains(string.Format(@"Tag=""{0}""", match.Value)))
                            break;
                        else
                        {
                            value = value.Replace(url, "<InlineUIContainer><TextBlock Foreground=\"Black\" Tag=\"" + match.Value + "\"><Underline>网页链接</Underline></TextBlock></InlineUIContainer>");
                        }
                    }

                    foreach (Match m in hastagmc)
                    {
                        if (value.Contains(string.Format(@"<Underline>{0}</Underline>", m.Value)))
                            break;
                        value = value.Replace(m.Value, @"<InlineUIContainer><TextBlock Foreground=""Black""><Underline>" + m.Value + "</Underline></TextBlock></InlineUIContainer>");
                    }

                    foreach (Match m in usermc)
                    {
                        if (value.Contains(string.Format(@"<Underline>{0}</Underline>", m.Value)))
                            break;
                        value = value.Replace(m.Value, @"<InlineUIContainer><TextBlock Foreground=""Black""><Underline>" + m.Value + "</Underline></TextBlock></InlineUIContainer>");
                    }
                    var xaml = string.Format(@"<Paragraph 
                                        xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                        xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                                    <Paragraph.Inlines>
                                    <Run></Run>
                                        {0}
                                    </Paragraph.Inlines>
                                </Paragraph>", value);
                    var p = (Paragraph)XamlReader.Load(xaml);
                    control.Blocks.Add(p);
                }
            }
            catch (XamlParseException opp)
            {
                control.Blocks.Clear();
                string ErrorListStr = App.ErrorConversionString;
                List<string> ErrorList = new List<string>();
                if (ErrorListStr != "")
                {
                    ErrorList = JsonConvert.DeserializeObject<List<string>>(ErrorListStr);
                }
                if (!ErrorList.Contains(e.NewValue.ToString()))
                {
                    ContentDialog MesDia = new ContentDialog();
                    MesDia.Content = "字符转换出现了未识别的错误并且造成了应用的异常，是否想要反馈，若不反馈应用将自动推出";
                    MesDia.PrimaryButtonText = "好的";
                    MesDia.SecondaryButtonText = "不好";
                    ContentDialogResult result = await MesDia.ShowAsync();
                    if (result == ContentDialogResult.Primary)
                    {
                        EmailRecipient rec = new EmailRecipient("metrsignstudio@outlook.com");
                        EmailMessage mes = new EmailMessage();
                        mes.To.Add(rec);
                        mes.Subject = "微博正则表达式Bug反馈";
                        mes.Body = "          字符串内容：" + e.NewValue.ToString();
                        await EmailManager.ShowComposeNewEmailAsync(mes);
                        ErrorList.Add(e.NewValue.ToString());
                        ErrorListStr = JsonConvert.SerializeObject(ErrorList);
                        App.ErrorConversionString = ErrorListStr;
                    }
                    else
                    {
                        Application.Current.Exit();
                    }
                    
                }

                var xaml = string.Format(@"<Paragraph 
                                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                                <Paragraph.Inlines>
                                <Run></Run>
                                    {0}
                                </Paragraph.Inlines>
                            </Paragraph>", e.NewValue.ToString());
                var p = (Paragraph)XamlReader.Load(xaml);
                control.Blocks.Add(p);
            }
        }
    }
}
