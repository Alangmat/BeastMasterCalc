using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace View
{
    // Конвертер для преобразования строки в число с учетом разных разделителей
    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = value as string;
            if (string.IsNullOrWhiteSpace(stringValue))
                return 0;

            // Пытаемся разделить строку на части по разделителю точки или запятой
            string[] parts = stringValue.Split('.', ',');
            if (parts.Length == 2 && int.TryParse(parts[0], out int whole) && double.TryParse(parts[1], NumberStyles.Any, culture, out double fraction))
            {
                // Обе части успешно преобразовались в числа, считаем значение
                return whole + fraction / Math.Pow(10, parts[1].Length);
            }
            else if (double.TryParse(stringValue, NumberStyles.Any, culture, out double result))
            {
                // Если не удалось разделить строку, пробуем преобразовать как обычное число
                return result;
            }
            double res = 0;
            if (!Double.TryParse(value as string, out res))
            {
                return 0;
            }

            return DependencyProperty.UnsetValue;
        }
    }

}
