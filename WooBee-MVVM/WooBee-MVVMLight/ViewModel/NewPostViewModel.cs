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
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;

namespace WooBee_MVVMLight
{
    public class NewPostViewModel : ViewModelBase, INavigable
    {
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


        //private RelayCommand _emoticonPanelCommand;
        //public RelayCommand EmoticonPanelCommand
        //{
        //    get
        //    {
        //        if (_emoticonPanelCommand != null) return _emoticonPanelCommand;
        //        return _emoticonPanelCommand = new RelayCommand(() =>
        //        {
        //            TODO: Invoke Emoticon Panel
        //        });
        //    }
        //}


        //private RelayCommand _callImageCommand;
        //public RelayCommand CallImageCommand
        //{
        //    get
        //    {
        //        if (_callImageCommand != null) return _callImageCommand;
        //        return _callImageCommand = new RelayCommand(() =>
        //        {
        //            //TODO: Display image button
        //        });
        //    }
        //}

        //private RelayCommand _returnButtonCommand;
        //public RelayCommand ReturnButtomCommand
        //{
        //    get
        //    {
        //        if (_returnButtonCommand != null) return _returnButtonCommand;
        //        return _returnButtonCommand = new RelayCommand(() =>
        //        {
        //            //TODO: Return to previous state
        //        });
        //    }
        //}

        private RelayCommand _invokeCameraCommand;
        public RelayCommand InvokeCameraCommand
        {
            get
            {
                if (_invokeCameraCommand != null) return _invokeCameraCommand;
                return _invokeCameraCommand = new RelayCommand(() =>
                {
                    //TODO: Invoke camera feature
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

        private static async Task OpenPictureLibrary()
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
        }

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

        private ObservableCollection<BitmapImage> _emojisource = App._EmoticonSource;
        public ObservableCollection<BitmapImage> EmojiSource
        {
            get
            {
                return _emojisource;
            }
        }

        public NewPostViewModel() { }

        private static IReadOnlyList<StorageFile> files { get; set; }

        private async Task PostWeibo(string textBlockString)
        {
            try
            {
                if (files != null)
                {
                    string posturi = "https://api.weibo.com/2/statuses/upload_url_text.json?access_token=" + App.WeicoAccessToken;
                    foreach (StorageFile file in files)
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
            }
            catch(Exception e)
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

        private bool IsFirst = true;
        private string pic_ids;
    }
}
