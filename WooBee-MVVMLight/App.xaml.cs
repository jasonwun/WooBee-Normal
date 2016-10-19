using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Windows.Storage;
using WooBee_MVVMLight.View;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using Windows.Phone.UI.Input;
using GalaSoft.MvvmLight.Views;
using WooBee_MVVMLight.Common;

namespace WooBee_MVVMLight
{
    sealed partial class App
    {

        public static StringBuilder builder = new StringBuilder();
        public static string AccessToken
        {
            get
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                if (localSettings.Values["NormalAccessToken"] == null)
                    return "";
                else
                    return localSettings.Values["NormalAccessToken"].ToString();
            }
            set
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["NormalAccessToken"] = value;
            }
        }
        public static string WeicoAccessToken
        {
            get
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                if (localSettings.Values["WeicoAccessToken"] == null)
                    return "";
                else
                    return localSettings.Values["WeicoAccessToken"].ToString();
            }
            set
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["WeicoAccessToken"] = value;
            }
        }
        public static string ErrorConversionString
        {
            get
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                if (localSettings.Values["ErrorStringConverstion"] == null)
                    return "";
                else
                    return localSettings.Values["ErrorStringConverstion"].ToString();
            }
            set
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["ErrorStringConverstion"] = value;
            }
        }
        public static string Uid
        {
            get
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                if (localSettings.Values["Uid"] == null)
                    return "";
                else
                    return localSettings.Values["Uid"].ToString();
            }
            set
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["Uid"] = value;
            }
        }
        public static string AppKey = "839927271";
        public static string AppSecret = "d9a2ae8a01ef87772897bcf0c32ea575";
        public static AppSettings AppSettings
        {
            get
            {
                return App.Current.Resources["AppSettings"] as AppSettings;
            }
        }
        public static ObservableCollection<BitmapImage> _EmoticonSource = new ObservableCollection<BitmapImage>();
        public static StorageFile Photofile { get; set; }
        public static int Since_id = 0;
        public static double _scrollViewerVerticalOffset = 0;
        public static bool IsRefresh = true;
        public static Dictionary<string, string> _reverseDict = new Dictionary<string, string>();

        #region EmojiDict
        public static Dictionary<string, string> emojiDict = new Dictionary<string, string>
    {
            {"[最右]","01" },{ "[微笑]","02" },{ "[嘻嘻]","03" },{ "[哈哈]","04" },{ "[爱你]","05" },{ "[挖鼻]","06" },{ "[吃惊]","07" },{"[晕]","08" },
            { "[泪]","09" },{ "[馋嘴]","10" },{ "[抓狂]","11" },{ "[哼]","12" },{ "[可爱]","13" },{ "[怒]","14" },{"[汗]","15" },{ "[害羞]","16" },
            { "[睡]","17" }, { "[钱]","18" },{ "[偷笑]","19" },{ "[笑cry]","20" },{ "[doge]","21" },{ "[喵喵]","22" },{"[酷]","23" },{ "[衰]","24" },
            { "[闭嘴]","25" },{ "[鄙视]","26" },{ "[色]","27" },{ "[鼓掌]","28" },{ "[悲伤]","29" },{"[思考]","30" },{ "[生病]","31" },{ "[亲亲]","32" },
            { "[怒骂]","33" },{ "[太开心]","34" },{ "[白眼]","35" },{ "[右哼哼]","36" },{"[左哼哼]","37" },{ "[嘘]","38" },{ "[委屈]","39" },{"[吐]","40" },
            {"[可怜]","41" },{"[哈欠]","42" },{"[挤眼]","43" },{"[失望]","44" },{"[顶]","45" },{"[疑问]","46" },{"[困]","47" },{"[感冒]","48" },{"[拜拜]","49" },
            {"[黑线]","50" },{"[阴险]","51" },{"[互粉]","52" },{"[心]","53" },{"[伤心]","54" },{"[猪头]","55" },{"[熊猫]","56" },{"[兔子]","57" },{"[握手]","58" },
            {"[作揖]","59" },{"[赞]","60" },{"[耶]","61" },{"[good]","62" },{"[弱]","63" },{"[NO]","64" },{"[ok]","65" },{"[haha]","66" },{"[来]","67" },
            {"[拳头]","68" },{"[威武]","69" },{"[鲜花]","70" },{"[钟]","71" },{"[浮云]","72" },{"[飞机]","73" },{"[月亮]","74" },{"[太阳]","75" },{"[微风]","76" },
            {"[下雨]","77" },{"[给力]","78" },{"[神马]","79" },{"[围观]","80" },{"[话筒]","81" },{"[奥特曼]","82" },{"[草泥马]","83" },{"[萌]","84" },
            {"[囧]","85" },{"[织]","86" },{"[礼物]","87" },{"[喜]","88" },{"[围脖]","89" },{"[音乐]","90" },{"[绿丝带]","91" },{"[蛋糕]","92" },{"[蜡烛]","93" },
            {"[干杯]","94" },{"[男孩儿]","95" },{"[女孩儿]","96" },{"[肥皂]","97" },{"[照相机]","98" },{"[浪]","99" },{"😃","100" },{"😍","101" },{"😒","102" },
            {"😳","103" },{"😁","104" },{"😘","105" },{"😉","106" },{"😠","107" },{"😞","108" },{"😥","109" },{"😭","110" },{"😝","111" },{"😡","112" },
            {"😣","113" },{"😔","114" },{"😄","115" },{"😷","116" },{"😚","117" },{"😓","118" },{"😂","119" },{"😊","120" },{"😢","121" },
            {"😜","122" },{"😨","123" },{"😰","124" },{"😲","125" },{"😏","126" },{"😱","127" },{"😪","128" },{"😖","129" },{"😌","130" },{"👿","131" },
            {"👻","132" },{"🎅","133" },{"👧","134" },{"👦","135" },{"👩","136" },{"🐶","138" },{"👍","140" },{"👎","141" },{"👊","142" },{"✊","143" },{"✌","144" },
            {"💪","145" },{"👏","146" },{"👈","147" },{"👆","148" },{"👉","149" },{"👇","150" },{"👌","151" },{"❤","152" },{"💔","153" },{"🙏","154" },
            {"☀","155" },{"🌙","156" },{"🌟","157" },{"⚡","158" },{"☁","159" },{"☔","160" },{"🍁","161" },{"🌻","162" },{"🍃","163" },{"👗","164" },
            {"🎀","165" },{"👄","166" },{"🌹","167" },{"☕","168" },{"🎂","169" },{"🕙","170" },{"🍺","171" },{"🔍","172" }, {"📱","173" },{"🏠","174" },{"🚗","175" },
            {"🎁","176" },{"💣","177" },{"⚽","178" },{"[笑哈哈]","179" },{"[好爱哦]","180" },{"[噢耶]","181" },{"[偷乐]","182" },{"[泪流满面]","183" },{"[巨汗]","184" },
            {"[抠鼻屎]","185" },{"[求关注]","186" },{"[好喜欢]","187" },{"[崩溃]","188" },{"[好囧]","189" },{"[震惊]","190" }, {"[别烦我]","191" },
            {"[不好意思]","192" },{"[羞嗒嗒]","193" },{"[得意地笑]","194" },{"[纠结]","195" },{"[给劲]","196" },{"[悲催]","197" },{"[甩甩手]","198" },{"[好棒]","199" },{"[瞧瞧]","200" },{"[不想上班]","201" },
            {"[困死了]","202" },{"[许愿]","203" },{"[丘比特]","204" },{"[有鸭梨]","205" },{"[想一想]","206" },{"[躁狂症]","207" },{"[转发]","208" },{"[互相膜拜]","209" },
            {"[雷锋]","210" },{"[杰克逊]","211" },{"[玫瑰]","212" },{"[hold住]","213" },{"[群体围观]","214" },{"[推荐]","215" },{"[赞啊]","216" },{"[被电]","217" },
        };
        #endregion

        private void StringBuilder()
        {
            foreach (var key in emojiDict.Keys)
            {
                builder.Append(key.Replace("[", @"\[").Replace("]", @"\]"));

                builder.Append("|");
            }
            builder.Remove(builder.Length - 1, 1);
        }

        private void LoadEmoticons()
        {
            foreach (KeyValuePair<string, string> qwe in emojiDict)
            {
                string a = string.Format("ms-appx:///Assets/Emoticon/{0}.png", qwe.Value);
                BitmapImage item = new BitmapImage(new Uri(a));
                _EmoticonSource.Add(item);
            }
        }

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            LoadEmoticons();
            StringBuilder();
        }

        

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.Navigated += OnNavigated;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;

                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    rootFrame.CanGoBack ?
                    AppViewBackButtonVisibility.Visible :
                    AppViewBackButtonVisibility.Collapsed;

            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (App.AccessToken != "" && App.WeicoAccessToken != "")
                    rootFrame.Navigate(typeof(TimeLineView), e.Arguments);
                else
                    rootFrame.Navigate(typeof(LoginView), e.Arguments);
                //rootFrame.Navigate(typeof(BlankPage1), e.Arguments);
            }


            // Ensure the current window is active
            Window.Current.Activate();
            DispatcherHelper.Initialize();

            Messenger.Default.Register<NotificationMessageAction<string>>(
                this,
                HandleNotificationMessage);

        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack && rootFrame.CurrentSourcePageType != typeof(TimeLineView))
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
            
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack && rootFrame.CurrentSourcePageType != typeof(TimeLineView))
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
           
        }



        private void HandleNotificationMessage(NotificationMessageAction<string> message)
        {
            message.Execute("Success (from App.xaml.cs)!");
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
