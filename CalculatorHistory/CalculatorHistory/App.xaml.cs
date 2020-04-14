using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CalculatorHistory
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static ImageSourceConverter ImageSourceConverter { get; } = new ImageSourceConverter();
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			DictionaryKeyToValueConverter conv = (DictionaryKeyToValueConverter)Resources["DictionaryKeyToValueConverter"];
			conv.SetDictionary(new Dictionary<string, ImageSource>()
			{
				{"Addition" , (ImageSource) ImageSourceConverter.ConvertFrom(@"Images\plus.png")},
				{"Subtraction" ,(ImageSource) ImageSourceConverter.ConvertFrom(@"Images\subt.png")},
				{"Multiplication" ,(ImageSource) ImageSourceConverter.ConvertFrom(@"Images\multi.png")},
				{"Division" ,(ImageSource) ImageSourceConverter.ConvertFrom(@"Images\div.png")}
			});
		}
	}
}
