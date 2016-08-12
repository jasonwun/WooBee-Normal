using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;

namespace WooBee_MVVMLight
{
    public class ScalingHelper
    {
        DisplayInformation displayinformation { get; set; }

        public ScalingHelper()
        {
            displayinformation = DisplayInformation.GetForCurrentView();
        }

        public double SetTimeLineMultiImgsSize()
        {
            double rawPixelsPerViewPixel = displayinformation.RawPixelsPerViewPixel;
            double ScreenWidthRawPixels = displayinformation.ScreenWidthInRawPixels;
            double ImgSizeRawPixels = Math.Floor(ScreenWidthRawPixels * 0.27431);
            double ImgSizeViewPixels = ImgSizeRawPixels / rawPixelsPerViewPixel;
            return ImgSizeViewPixels;
        }
    }
}
