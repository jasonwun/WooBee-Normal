using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace WooBee_Normal
{
    public class ImageBindingHelper : DependencyObject
    {
        public static Uri GetUriProperty(DependencyObject obj)
        {
            return (Uri)obj.GetValue(UriPropertyProperty);
        }

        public static void SetUriProperty(DependencyObject obj, Uri value)
        {
            obj.SetValue(UriPropertyProperty, value);
        }

        // Using a DependencyProperty as the backing store for UriProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UriPropertyProperty =
            DependencyProperty.RegisterAttached("UriProperty", typeof(Uri), typeof(ImageBindingHelper), new PropertyMetadata(null, OnPropertyChanged));



        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var img = d as Image;
            if (img != null)
            {
                BitmapImage bitimg = new BitmapImage();
                bitimg.UriSource = new Uri(e.NewValue.ToString());
                img.Source = bitimg;
            }
        }
    }
}
