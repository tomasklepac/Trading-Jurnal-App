using System.Globalization;

namespace TradingJournalApp.Converters
{
    /// <summary>
    /// Converter that converts a boolean value to a color.
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a color.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool b)
                return b ? Color.FromArgb("#00FF99") : Colors.Gray;

            return Colors.White;
        }

        /// <summary>
        /// Converts a color back to a boolean value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
