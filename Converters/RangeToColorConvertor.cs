using System.Globalization;
using TradingJournalApp.Models;

namespace TradingJournalApp.Converters
{
    /// <summary>
    /// Converter that converts a DateRangeType to a color.
    /// </summary>
    public class RangeToColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a DateRangeType to a color.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateRangeType selected && parameter is string name)
            {
                if (Enum.TryParse(name, out DateRangeType target))
                {
                    return selected == target ? Color.FromArgb("#2BAA88") : Color.FromArgb("#1A1A1A");
                }
            }

            return Color.FromArgb("#1A1A1A");
        }

        /// <summary>
        /// Converts a color back to a DateRangeType.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}