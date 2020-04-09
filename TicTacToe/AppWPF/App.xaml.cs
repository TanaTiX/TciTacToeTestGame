using Common;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using View;

namespace AppWPF
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly Window window = new Window();
		private readonly FirstScreenUC firstScreenUC;
		private readonly StatisticUC statisticUC;
		private void OnStartUp(object sender, StartupEventArgs e)
		{
			window.Width = 600;
			window.Height = 700;
			GetType(typeof(FirstScreenVM));
		}
		private void GetType(Type type)
		{
			

			switch (type)
			{
				case typeof(IFirstScreenVM):
					window.Content = new FirstScreenVM();
					break;
				default:
					break;
			}
			window.Show();
		}
	}
}
