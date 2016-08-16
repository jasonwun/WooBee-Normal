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
                    string responseString = await response.Content.ReadAsStringAsync();
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
                    string responseString = await response.Content.ReadAsStringAsync();
                }

                SendOut.Clear();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
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
                if (IsFirst)
                {
                    IsFirst = false;
                    return "写点啥吧";
                }

                else
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

        public NewPostViewModel() { }
        #endregion


    }
}
