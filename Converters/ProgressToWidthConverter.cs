using System.Globalization;
using Microsoft.Maui.Controls;

namespace EvolutionPlayMobile.Converters;

public class ProgressToWidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double progress || progress < 0)
        {
            return 0;
        }

        if (parameter is not string maxWidthString || !double.TryParse(maxWidthString, out var maxWidth))
        {
            maxWidth = 260;
        }

        return Math.Max(0, Math.Min(maxWidth, progress * maxWidth));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
