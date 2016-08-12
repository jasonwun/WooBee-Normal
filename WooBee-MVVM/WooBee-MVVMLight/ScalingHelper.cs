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
        DisplayInformation displayInformation { get; set; }

        public ScalingHelper()
        {
            displayInformation = DisplayInformation.GetForCurrentView();
        }

        public double SetTimeLineMultiImgsSize()
        {
            var assist = new ScaleAssist.Scale();
            var res = assist.ScreenResolution;
            double rawPixelsPerViewPixel = displayInformation.RawPixelsPerViewPixel;
            double ScreenWidthRawPixels = res.X;
            double ImgSizeRawPixels = Math.Floor(ScreenWidthRawPixels * 0.26662);
            double ImgSizeViewPixels = ImgSizeRawPixels / rawPixelsPerViewPixel;
            return ImgSizeViewPixels;
        }
    }
}
