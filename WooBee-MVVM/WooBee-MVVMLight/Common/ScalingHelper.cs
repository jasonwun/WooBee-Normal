using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.Graphics.Display;

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
                double ImgSizeRawPixels = Math.Floor(ScreenWidthRawPixels * 0.27131);
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
    }
}
