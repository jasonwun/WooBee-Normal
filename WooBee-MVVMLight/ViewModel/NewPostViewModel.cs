using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;
using WooBee_MVVMLight.Common;
using WooBee_MVVMLight.View;

namespace WooBee_MVVMLight
{
    public class NewPostViewModel : ViewModelBase, INavigable
    {

        #region Command
        private RelayCommand _sendWeiboCommand;
        public RelayCommand SendWeiboCommand
        {
            get
            {
                if (_sendWeiboCommand != null) return _sendWeiboCommand;
                return _sendWeiboCommand = new RelayCommand(async () =>
                {
                    await PostWeibo(TextBlockString);
                });
            }

        }

        private RelayCommand _invokeCameraCommand;
        public RelayCommand InvokeCameraCommand
        {
            get
            {
                if (_invokeCameraCommand != null) return _invokeCameraCommand;
                return _invokeCameraCommand = new RelayCommand(async () =>
                {
                    var camera = new CameraCaptureUI();
                    camera.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
                    camera.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.Large3M;
                    var photo = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);

                    if (photo != null)
                    {
                        AddImageFileToData(photo);
                    }
                });
            }
        }

        private RelayCommand _openPhotoLibraryCommand;
        public RelayCommand OpenPhotoLibraryCommand
        {
            get
            {
                if (_openPhotoLibraryCommand != null) return _openPhotoLibraryCommand;
                return _openPhotoLibraryCommand = new RelayCommand(async () =>
                {
                    await OpenPictureLibrary();
                });
            }
        }
        #endregion

        #region Method
        private async Task OpenPictureLibrary()
        {
            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            open.ViewMode = PickerViewMode.Thumbnail;

            // Filter to include a sample subset of file types
            open.FileTypeFilter.Clear();
            open.FileTypeFilter.Add(".gif");
            open.FileTypeFilter.Add(".png");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".jpg");

            // Open a stream for the selected file
            files = await open.PickMultipleFilesAsync();
            if(files.Count > 9)
            {
                await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    MessageDialog a = new MessageDialog("照片数量不得超过9张图片");
                    a.Commands.Add(new UICommand("OK"));
                    await a.ShowAsync();
                });
            }
            else
            {
                for (int i = 0; i < files.Count; i++)
                {
                    AddImageFileToData(files[i]);
                }
            }
          
        }

        private async void AddImageFileToData(StorageFile a)
        {
            if (SendOut.Count == 0)
            {
                var stream = (FileRandomAccessStream)await a.OpenAsync(FileAccessMode.Read);
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(stream);
                GetFirstImg = bitmapImage;
            }
            SendOut.Add(a);
            int p = SendOut.Count;
            ImagesCount = p.ToString();
        }

        private async Task PostWeibo(string textBlockString)
        {
            if (PostType == "新微博")
            {
                await PostNewWeibo(textBlockString);
            }
            else if (PostType == "新评论")
            {
                await PostNewComment(textBlockString);
            }
            else if (PostType == "新转发")
            {
                await PostNewRepost(textBlockString);
            }
            else if(PostType == "回复评论")
            {
                await PostReplyComment(textBlockString);
            }
                else if(PostType == "回复转发")
            {
                await PostNewComment(textBlockString);
            }
        }

        private async Task PostReplyComment(string textBlockString)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string posturi = API.COMMENTS_REPLY;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(posturi));
                HttpFormUrlEncodedContent postData = new HttpFormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("access_token",App.AccessToken),
                        new KeyValuePair<string, string>("comment", textBlockString),
                        new KeyValuePair<string, string>("cid",CommentID),
                        new KeyValuePair<string, string>("id", WeiboID)
                    }
                );
                request.Content = postData;
                HttpResponseMessage response = await httpClient.SendRequestAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    ToastService.SendToast("发送成功", 1000);
                    await Task.Delay(600);
                    NavigationService.GoBack();
                }
                else
                {
                    StatusCodeDetermine(response);
                }
            }
            catch (Exception e)
            {
                ToastService.SendToast("Network not available", 1000);
            }


        }

        private async Task PostNewRepost(string textBlockString)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string posturi = API.REPOST;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(posturi));
                HttpFormUrlEncodedContent postData = new HttpFormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("access_token",App.AccessToken),
                        new KeyValuePair<string, string>("status", textBlockString),
                        new KeyValuePair<string, string>("id", WeiboID)
                    }
                );
                request.Content = postData;
                HttpResponseMessage response = await httpClient.SendRequestAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    ToastService.SendToast("发送成功", 1000);
                    await Task.Delay(600);
                    NavigationService.GoBack();
                }
                else
                {
                    StatusCodeDetermine(response);
                }
            }
            catch (Exception e)
            {
                ToastService.SendToast("Network not available", 1000);
            }

        }

        private async Task PostNewComment(string textBlockString)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string posturi = API.COMMENTS_CREATE;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(posturi));
                HttpFormUrlEncodedContent postData = new HttpFormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("access_token",App.AccessToken),
                        new KeyValuePair<string, string>("comment", textBlockString),
                        new KeyValuePair<string, string>("id", WeiboID)
                    }
                );
                request.Content = postData;
                HttpResponseMessage response = await httpClient.SendRequestAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    ToastService.SendToast("发送成功", 1000);
                    await Task.Delay(600);
                    NavigationService.GoBack();
                }
                else
                {
                    StatusCodeDetermine(response);
                }
            }
            catch (Exception e)
            {
                ToastService.SendToast("Network not available", 1000);
            }

        }

        private async Task PostNewWeibo(string textBlockString)
        {
            try
            {
                if (files != null)
                {
                    
                        string posturi = "https://api.weibo.com/2/statuses/upload_url_text.json?access_token=" + App.WeicoAccessToken;
                        foreach (StorageFile file in SendOut)
                        {
                            FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);
                            HttpStreamContent streamContent1 = new HttpStreamContent(stream);
                            await UploadImages(streamContent1);

                        }
                        HttpClient httpClient = new HttpClient();
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(posturi));
                        HttpFormUrlEncodedContent postData = new HttpFormUrlEncodedContent(
                            new List<KeyValuePair<string, string>>
                            {
                        new KeyValuePair<string, string>("status", textBlockString),
                        new KeyValuePair<string, string>("pic_id", pic_ids),

                            }
                        );
                        request.Content = postData;
                        HttpResponseMessage response = await httpClient.SendRequestAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        ToastService.SendToast("发送成功", 1000);
                        await Task.Delay(600);
                        NavigationService.GoBack();
                    }
                    else
                    {
                        StatusCodeDetermine(response);
                    }

                }
                else
                {
                    HttpClient httpClient = new HttpClient();
                    string posturi = "https://api.weibo.com/2/statuses/update.json?";
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(posturi));
                    HttpFormUrlEncodedContent postData = new HttpFormUrlEncodedContent(
                        new List<KeyValuePair<string, string>>
                        {
                        new KeyValuePair<string, string>("access_token",App.AccessToken),
                        new KeyValuePair<string, string>("status", textBlockString),

                        }
                    );
                    request.Content = postData;
                    HttpResponseMessage response = await httpClient.SendRequestAsync(request);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        ToastService.SendToast("发送成功", 1000);
                        await Task.Delay(600);
                        NavigationService.GoBack();
                    }
                    else
                    {
                        StatusCodeDetermine(response);
                    }
                }

                SendOut.Clear();
            }
            catch(Exception e)
            {
                ToastService.SendToast("Network not available", 1000);
            }
            
        }

        private static void StatusCodeDetermine(HttpResponseMessage response)
        {
            int StatusCode = (int)response.StatusCode;
            switch (StatusCode)
            {
                case 400:
                    ToastService.SendToast("400 Bad Request", 1000);
                    break;
                case 401:
                    ToastService.SendToast("401 Unauthorized", 1000);
                    break;
                case 403:
                    ToastService.SendToast("403 Forbidden", 1000);
                    break;
                case 404:
                    ToastService.SendToast("404 Not Found", 1000);
                    break;
                case 405:
                    ToastService.SendToast("405 Method Not Allowed", 1000);
                    break;
                case 406:
                    ToastService.SendToast("406 Not Acceptable", 1000);
                    break;
                case 407:
                    ToastService.SendToast("407 Proxy Authentication Required", 1000);
                    break;
                case 408:
                    ToastService.SendToast("408 Request Timeout", 1000);
                    break;
                case 409:
                    ToastService.SendToast("409 Conflict", 1000);
                    break;
                case 410:
                    ToastService.SendToast("410 Gone", 1000);
                    break;
                case 411:
                    ToastService.SendToast("411 Length Required", 1000);
                    break;
                case 412:
                    ToastService.SendToast("412 Precondition Failed", 1000);
                    break;
                case 413:
                    ToastService.SendToast("413 Request Entity Too Large", 1000);
                    break;
                case 414:
                    ToastService.SendToast("414 Request-URI Too Long", 1000);
                    break;
                case 415:
                    ToastService.SendToast("415 Unsupported Media Type", 1000);
                    break;
                case 416:
                    ToastService.SendToast("416 Requested Range Not Satisfiable", 1000);
                    break;
                case 417:
                    ToastService.SendToast("417 Expectation Failed", 1000);
                    break;
                case 422:
                    ToastService.SendToast("422 Unprocessable Entity", 1000);
                    break;
                case 423:
                    ToastService.SendToast("423 Locked", 1000);
                    break;
                case 424:
                    ToastService.SendToast("424 Failed Dependency", 1000);
                    break;
                case 425:
                    ToastService.SendToast("425 Unordered Collection", 1000);
                    break;
                case 426:
                    ToastService.SendToast("426 Upgrade Required", 1000);
                    break;
                case 449:
                    ToastService.SendToast("449 Retry With", 1000);
                    break;
                case 500:
                    ToastService.SendToast("500 Internal Server Error", 1000);
                    break;
                case 501:
                    ToastService.SendToast("501 Not Implemented", 1000);
                    break;
                case 502:
                    ToastService.SendToast("502 Bad Gateway", 1000);
                    break;
                case 503:
                    ToastService.SendToast("503 Service Unavailable", 1000);
                    break;
                case 504:
                    ToastService.SendToast("504 Gateway Timeout", 1000);
                    break;
                case 505:
                    ToastService.SendToast("505 HTTP Version Not Supported", 1000);
                    break;
                case 506:
                    ToastService.SendToast("506 Variant Also Negotiates", 1000);
                    break;
                case 507:
                    ToastService.SendToast("507 Insufficient Storage", 1000);
                    break;
                case 509:
                    ToastService.SendToast("509 Bandwidth Limit Exceeded", 1000);
                    break;
                case 510:
                    ToastService.SendToast("510 Not Extended", 1000);
                    break;
                case 600:
                    ToastService.SendToast("600 Unparseable Response Headers", 1000);
                    break;

            }
        }

        private async Task UploadImages(HttpStreamContent streamContent)
        {
            try
            {
                string posturi = "https://upload.api.weibo.com/2/statuses/upload_pic.json?access_token=" + App.WeicoAccessToken;
                HttpClient httpClient = new HttpClient();
                HttpMultipartFormDataContent fileContent = new HttpMultipartFormDataContent();
                fileContent.Add(streamContent, "pic", "pic.jpg");
                HttpResponseMessage response = await httpClient.PostAsync(new Uri(posturi), fileContent);
                string a = await response.Content.ReadAsStringAsync();
                JObject o = JObject.Parse(a);
                pic_ids += (string)o["pic_id"];
                pic_ids += ",";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

        }

        public void Activate(object parameter) { }

        public void Deactivate(object parameter) { }

        public void OnLoaded() { }
        #endregion

        #region Field & Constructor
        private string _textBlockString;
        public string TextBlockString
        {
            get
            {
                if (_textBlockString == null)
                    return "写点啥吧";
                return _textBlockString;
            }
            set
            {
                if (_textBlockString != value)
                    _textBlockString = value;
                RaisePropertyChanged(() => TextBlockString);
            }
        }

        private Image _image;
        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (_image != value)
                    _image = value;
                RaisePropertyChanged(() => Image);
            }
        }

        private string _imagesCount;
        public string ImagesCount
        {
            get
            {
                return _imagesCount;
            }
            set
            {
                if (_imagesCount != value)
                    _imagesCount = value;
                RaisePropertyChanged(() => ImagesCount);
            }
        }

        private BitmapImage _getFirstImg;
        public BitmapImage GetFirstImg
        {
            get
            {
                return _getFirstImg;
            }
            set
            {
                if (_getFirstImg != value)
                    _getFirstImg = value;
                RaisePropertyChanged(() => GetFirstImg);
                NewPhotosInserted?.Invoke();
            }
        }

        private string _postType;
        public string PostType
        {
            get
            {
                if (_postType == "post")
                    return "新微博";
                else if (_postType == "comment")
                    return "新评论";
                else if (_postType == "repost")
                    return "新转发";
                else if (_postType == "replycomment")
                    return "回复评论";
                else if (_postType == "replyrepost")
                    return "回复转发";
                else
                    return "";
            }
            set
            {
                if(_postType != value)
                {
                    _postType = value;
                }
                RaisePropertyChanged(() => PostType);
            }
        }

        private string _weiboID;
        public string WeiboID
        {
            get
            {
                return _weiboID;
            }
            set
            {
                if (_weiboID != value)
                    _weiboID = value;
                RaisePropertyChanged(() => WeiboID);
            }
        }

        private string _commentID;
        public string CommentID
        {
            get
            {
                return _commentID;
            }
            set
            {
                if (_commentID != value)
                    _commentID = value;
                RaisePropertyChanged(() => CommentID);
            }
        }

        private bool _isPhotoButtonShown;
        public bool IsPhotoButtonShown
        {
            get
            {
                return _isPhotoButtonShown;
            }
            set
            {
                if (_isPhotoButtonShown != value)
                    _isPhotoButtonShown = value;
                RaisePropertyChanged(() => IsPhotoButtonShown);
            }
        }

        private ObservableCollection<BitmapImage> _emojisource = App._EmoticonSource;
        public ObservableCollection<BitmapImage> EmojiSource
        {
            get
            {
                return _emojisource;
            }
        }

        private static IReadOnlyList<StorageFile> files { get; set; }
        private List<StorageFile> SendOut = new List<StorageFile>();
        

        private bool IsFirst = true;
        private string pic_ids;
        public event Action NewPhotosInserted;

        public NewPostViewModel(string posttype, string weiboID, string commentID = null)
        {
            PostType = posttype;
            WeiboID = weiboID;
            CommentID = commentID;
        }
        #endregion


    }
}
