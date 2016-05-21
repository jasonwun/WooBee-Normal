using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Xaml;

namespace WooBee_Normal
{
    public class DisplayProperty
    {
        public DisplayProperty()
        {
            DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();
            UpdateDisplayProperty(displayInformation);
            
        }

        private void UpdateDisplayProperty(DisplayInformation displayInformation)
        {
            string abc = "";
        }

        // Helpers to convert between points and pixels.
        private double PtFromPx(double pixel)
        {
            return pixel * 72 / 96;
        }

        private double PxFromPt(double pt)
        {
            return pt * 96 / 72;
        }

        //void OutputSettings(double rawPixelsPerViewPixel, FrameworkElement rectangle, TextBlock viewPxText, TextBlock rawPxText, TextBlock fontTextBlock)
        //{
        //    // Get the size of the rectangle in view pixels and calulate the size in raw pixels.
        //    double sizeInViewPx = rectangle.Width;
        //    double sizeInRawPx = sizeInViewPx * rawPixelsPerViewPixel;

        //    viewPxText.Text = sizeInViewPx.ToString("F1") + " view px";
        //    rawPxText.Text = sizeInRawPx.ToString("F0") + " raw px";

        //    double fontSize = PtFromPx(fontTextBlock.FontSize);
        //    fontTextBlock.Text = fontSize.ToString("F0") + "pt";
        //}

        //void ResetOutput()
        //{
        //    DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();
        //    double rawPixelsPerViewPixel = displayInformation.RawPixelsPerViewPixel;

        //    // Set the override rectangle size and override text font size by taking our desired
        //    // size in raw pixels and converting it to view pixels.
        //    const double rectSizeInRawPx = 100;
        //    double rectSizeInViewPx = rectSizeInRawPx / rawPixelsPerViewPixel;
        //    OverrideLayoutRect.Width = rectSizeInViewPx;
        //    OverrideLayoutRect.Height = rectSizeInViewPx;

        //    double fontSizeInRawPx = PxFromPt(20);
        //    double fontSizeInViewPx = fontSizeInRawPx / rawPixelsPerViewPixel;
        //    OverrideLayoutText.FontSize = fontSizeInViewPx;

        //    // Output settings for controls with default scaling behavior.
        //    OutputSettings(rawPixelsPerViewPixel, DefaultLayoutRect, DefaultRelativePx, DefaultPhysicalPx, DefaultLayoutText);
        //    // Output settings for override controls.
        //    OutputSettings(rawPixelsPerViewPixel, OverrideLayoutRect, OverrideRelativePx, OverridePhysicalPx, OverrideLayoutText);
        //}
    }

}
