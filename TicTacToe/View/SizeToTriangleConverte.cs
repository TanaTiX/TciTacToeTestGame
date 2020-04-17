using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace View
{
	[ValueConversion(typeof(double[]), typeof(Point))]
	internal class SizeToTriangleConverte : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values?.Length == 2
				&& double.TryParse(values[0].ToString(), out double w) && double.TryParse(values[1].ToString(), out double h)
				&& int.TryParse(parameter.ToString(), out int v))
			{
				switch (v)
				{
					case 0: return new Point(w / 2.0, 0.0);
					case 1: return new Point(0.0, h);
					case 2: return new Point(w, h);
				}

			}
			return null;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
