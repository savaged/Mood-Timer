using System;
using System.Globalization;
using Xamarin.Forms;

namespace MoodTimer
{
    public class MoodToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colour = Color.Default;
            if (value is int mood)
            {
                int a = 255 - mood;
                colour = Color.FromRgba(mood, 0, 0, a);
            }
            return colour;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
