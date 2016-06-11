using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WooBee_Normal
{
    public sealed partial class SingleImage : UserControl
    {
        public SingleImage()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register("Source", typeof(ImageSource), typeof(SingleImage),
            new PropertyMetadata(default(ImageSource), SourceChanged));

        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.Register("PlaceHolder", typeof(ImageSource), typeof(SingleImage),
            new PropertyMetadata(default(ImageSource)));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public ImageSource PlaceHolder
        {
            get { return (ImageSource)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }

        private static void SourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = (SingleImage)dependencyObject;
            var newSource = (BitmapImage)dependencyPropertyChangedEventArgs.NewValue;

            //System.Diagnostics.Debug.WriteLine("Image source changed: {0}", ((BitmapImage)newSource).UriSource.AbsolutePath);


            if (newSource != null)
            {
                string temp = newSource.UriSource.ToString();
                temp = temp.Replace("thumbnail", "mw690");
                Uri imageUri = new Uri(temp);
                BitmapImage image = new BitmapImage(imageUri);
                string GifType = image.UriSource.ToString().Substring(image.UriSource.ToString().Length - 4);
                if (GifType == ".gif")
                {
                    control.GifWaterMark.Visibility = Visibility.Visible;
                }

                // If the image is not a local resource or it was not cached
                if (image.UriSource.Scheme != "ms-appx" && image.UriSource.Scheme != "ms-resource" && (image.PixelHeight * image.PixelWidth == 0))
                {
                    image.ImageOpened += (sender, args) => control.LoadImage(image);
                    control.Staging.Source = image;
                }
                else
                {
                    control.LoadImage(newSource);
                }
            }
        }

        private void LoadImage(ImageSource source)
        {
            ImageFadeOut.Completed += (s, e) =>
            {
                Image.Source = source;
                ImageFadeIn.Begin();
            };
            ImageFadeOut.Begin();
        }
    }
}
