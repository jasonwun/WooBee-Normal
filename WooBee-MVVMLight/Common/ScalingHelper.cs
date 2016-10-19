using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.Graphics.Display;
using Windows.UI.Xaml;

namespace WooBee_MVVMLight
{
    public class ScalingHelper
    {
        DisplayInformation displayInformation { get; set; }

        public ScalingHelper()
        {
            displayInformation = DisplayInformation.GetForCurrentView();
        }

        public double SetTimeLineMultiImgsSize()
        {
            try
            {
                double ScreenWidthRawPixels = GetWidth();
                double rawPixelsPerViewPixel = displayInformation.RawPixelsPerViewPixel;
                double ImgSizeRawPixels = ScreenWidthRawPixels * 0.267708333;
                double ImgSizeViewPixels = ImgSizeRawPixels / rawPixelsPerViewPixel;
                return ImgSizeViewPixels;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            return 0.0;
            
        }

        private double GetWidth()
        {
            var rect = PointerDevice.GetPointerDevices().Last().ScreenRect;
            var scale = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            return rect.Width * scale;
        }

        public double GetWindowWidth()
        {
            double ScreenWidthRawPixels = Window.Current.Bounds.Width;
            return ScreenWidthRawPixels;
        }

        public double GetWindowsHeight()
        {
            double ScreenHeightRawPixels = Window.Current.Bounds.Height;
            return ScreenHeightRawPixels;
        }
    }
}
