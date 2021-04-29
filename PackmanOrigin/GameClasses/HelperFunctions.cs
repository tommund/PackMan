using Windows.UI.Xaml.Controls;

namespace PackmanOrigin.GameClasses
{
    // General functions to help deal with images collison
    public static class HelperFunctions
    {
        public static bool IsImageCollidingWithOtherImage(Image image, double left, double top, Image otherImage)
        {
            double right = left + image.ActualWidth;
            double bottom = top + image.ActualHeight;
            // check left upper edge
            if (IsPointInsideImage(left, top, otherImage)) return true;
            // check right upper edge
            if (IsPointInsideImage(right, top, otherImage)) return true;
            // check left bottom edge
            if (IsPointInsideImage(left, bottom, otherImage)) return true;
            // check right bottom edge
            if (IsPointInsideImage(right, bottom, otherImage)) return true;

            return false;
        }
        public static bool IsPointInsideImage(double left, double top, Image image)
        {
            double imageLeft = Canvas.GetLeft(image);
            double imageTop = Canvas.GetTop(image);
            return ((left > imageLeft &&
                left < imageLeft + image.ActualWidth) &&
                (top > imageTop && top <
                imageTop + image.ActualHeight));
        }
    }
}
