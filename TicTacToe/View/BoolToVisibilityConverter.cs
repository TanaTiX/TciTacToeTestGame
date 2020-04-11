using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace View
{
	public class BoolToVisibilityConverter : IMultiValueConverter
	{
		/// <summary>
		/// Проверка на наличие фокуса и наличие текста в текстовом поле на основе стандартных параметров интерфейса IMultiValueConverter
		/// </summary>
		/// <param name="values"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns>true/false</returns>
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool hasFocus = (bool)values[0];
			bool hasText = !string.IsNullOrEmpty((string)values[1]);
			return (hasText || hasFocus) ? Visibility.Collapsed : Visibility.Visible;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
