using CommonUtils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace View
{
	public class UserSettingsSaveButtonConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if(values[0].ToString() != values[1].ToString())
			{
				return "Сохранить настройки и выйти";
			}
			else
			{
				return "Необходимо выбрать 2 разных игровых знака";
			}
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
