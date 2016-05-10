using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.DateTimeFormatting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WooBee_Normal
{
    public class TimingBindingHelper : DependencyObject
    {
        private static Dictionary<string, string> Weekdays = new Dictionary<string, string>()
        {
            {"Jan", "01" },
            {"Feb", "02" },
            {"Mar", "03" },
            {"Apr", "04" },
            {"May", "05" },
            {"Jun", "06" },
            {"Jul", "07" },
            {"Aug", "08" },
            {"Sep", "09" },
            {"Oct", "10" },
            {"Nov", "11" },
            {"Dec", "12" },
        };



        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(TimingBindingHelper), new PropertyMetadata(String.Empty, OnTextChanged));

        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as TextBlock;
            if (control != null)
            {
                string WeiboDateTime = e.NewValue.ToString();   //Tue May 31 17:46:55 +0800 2011
                WeiboDateTime = WeiboDateTimeUnifying(WeiboDateTime);//05 31 17:46:55 2011
                DateTime dateTime = DateTime.Now;
                TimeZoneInfo BeijingZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
                ObservableCollection<String> language = new ObservableCollection<string>();
                language.Add("en-US");
                DateTimeFormatter _formatter = new DateTimeFormatter("day month year hour minute second", language);
                string SystemDateTime = _formatter.Format(TimeZoneInfo.ConvertTime(dateTime, BeijingZone)); // May‎ ‎11‎, ‎2016‎ ‎3‎:‎38‎:‎10‎ ‎AM
                SystemDateTime = SystemDateTimeUnifying(SystemDateTime);
                string output = DateTimeCompare(WeiboDateTime, SystemDateTime);
                control.Text = output;
            }
        }

        private static string DateTimeCompare(string weiboDateTime, string systemDateTime)
        {
            string result = "";
            int weibo_month = Convert.ToInt32(weiboDateTime.Substring(0, 2));
            int syste_month = Convert.ToInt32(systemDateTime.Substring(0, 2));
            int weibo_day = Convert.ToInt32(weiboDateTime.Substring(3, 2));
            int syste_day = Convert.ToInt32(systemDateTime.Substring(3, 2));
            int weibo_hour = Convert.ToInt32(weiboDateTime.Substring(6, 2));
            int syste_hour = Convert.ToInt32(systemDateTime.Substring(6, 2));
            int weibo_minu = Convert.ToInt32(weiboDateTime.Substring(9, 2));
            int syste_minu = Convert.ToInt32(systemDateTime.Substring(9, 2));
            int weibo_seco = Convert.ToInt32(weiboDateTime.Substring(12, 2));
            int syste_seco = Convert.ToInt32(systemDateTime.Substring(12, 2));
            int weibo_year = Convert.ToInt32(weiboDateTime.Substring(15, 4));
            int syste_year = Convert.ToInt32(systemDateTime.Substring(15, 4));

            if (weibo_year == syste_year)
            {
                if (weibo_month == syste_month)
                {
                    if (weibo_day == syste_day)
                    {
                        if (weibo_hour == syste_hour)
                        {
                            if (weibo_minu == syste_minu)
                            {
                                result = (syste_seco - weibo_seco).ToString() + "s";
                            }
                            else
                            {
                                result = (syste_minu - weibo_minu).ToString() + "m";
                            }
                        }
                        else
                        {
                            result = (syste_hour - weibo_hour).ToString() + "h";
                        }
                    }
                    else
                    {
                        result = (syste_day - weibo_day).ToString() + "d";
                    }
                }
                else
                {
                    result = (syste_hour - weibo_hour).ToString() + "m";
                }
            }
            else
            {
                result = (syste_year - weibo_year).ToString() + "y";
            }

            return result;
        }

        private static string SystemDateTimeUnifying(string systemDateTime)
        {
            string tmp = new string(systemDateTime.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray()); //May11201634729AM
            string month = Weekdays[tmp.Substring(0, 3)];
            string day = tmp.Substring(3, 2);
            string year = tmp.Substring(5, 4);
            string APM = tmp.Substring(tmp.Length - 2);
            string hour = "";
            if (APM == "AM")
            {
                hour = "0" + tmp.Substring(9, 1);
            }
            else if (APM == "PM")
            {
                hour = (Convert.ToInt32(tmp.Substring(9, 1)) + 12).ToString();
            }
            string minutes = tmp.Substring(10, 2);
            string seconds = tmp.Substring(12, 2);
            string result = month + " " + day + " " + hour + ":" + minutes + ":" + seconds + " " + year;
            return result;
        }

        private static string WeiboDateTimeUnifying(string weiboDateTime)
        {
            string result = weiboDateTime;
            result = result.Substring(4); //May 31 17:46:55 +0800 2011
            result = result.Replace(result.Substring(0, 3), Weekdays[result.Substring(0, 3)]).Replace(" +0800", "");//05 31 17:46:55 2011
            return result;
        }
    }
}
