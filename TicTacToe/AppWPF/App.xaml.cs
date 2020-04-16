using Common;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using View;

namespace AppWPF
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		/// <summary>Экземпляр главного окна</summary>
		/// <remarks>Экземпляр создаётся один раз на всё время жизни приложения.
		/// При закрытии окна происходит закрытие приложения.</remarks>
		private readonly Window window = new Window()
		{
			WindowStartupLocation = WindowStartupLocation.CenterScreen,
			ResizeMode = ResizeMode.NoResize
		};

		/// <summary>Экземпляр Первого экрана</summary>
		/// <remarks>Экземпляр создаётся один раз на всё время жизни приложения</remarks>
		private readonly FirstScreenUC firstScreenUC = new FirstScreenUC();

		/// <summary>Экземпляр экрана Статистики</summary>
		/// <remarks>Экземпляр создаётся один раз на всё время жизни приложения</remarks>
		private readonly StatisticUC statisticUC = new StatisticUC();
		
		/// <summary>Экземпляр экрана Настроек</summary>
		/// <remarks>Экземпляр создаётся один раз на всё время жизни приложения</remarks>
		private readonly SettingsUC settingsUC = new SettingsUC();
		
		/// <summary>Экземпляр экрана собственно игры</summary>
		/// <remarks>Экземпляр создаётся один раз на всё время жизни приложения</remarks>
		private readonly GameUC gameUC = new GameUC();

		/// <summary>Экземпляр экрана собственно игры</summary>
		/// <remarks>Экземпляр создаётся один раз на всё время жизни приложения</remarks>
		private readonly GameEndUC gameEndUC = new GameEndUC();



		/// <summary>Словарь соответвия Контролов отображаемым типам</summary>
		private readonly Dictionary<Type, UserControl> controls 
			= new Dictionary<Type, UserControl>();

		private void OnStartUp(object sender, StartupEventArgs e)
		{
			///<remarks>Подсоединение обработчика закрытия окна</remarks>
			window.Closed += Window_Closed;

			///<remarks>Заполнение словаря соответствий</remarks>
			controls.Add(typeof(IFirstScreenVM), firstScreenUC);
			controls.Add(typeof(IStatisticVM), statisticUC);
			controls.Add(typeof(ISettingsVM), settingsUC);
			controls.Add(typeof(IGameVM), gameUC);
			controls.Add(typeof(IGameEndVM), gameEndUC);

			
			window.Width = 600;
			window.Height = 700;
			ChangeWindowContent(typeof(ISettingsVM));

			window.Show();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			///<remarks>Закрываем приложение</remarks>
			Shutdown();
		}

		/// <summary>Метод изменяющий Представление по задданому типу</summary>
		/// <param name="type">Тип который нужно представлять в окне</param>
		private void ChangeWindowContent(Type type)
		{
			///<remarks>Проверяем наличие ключа в словаре.
			///Если есть ключ, то в контент окна записываем его значение</remarks>
			if (controls.TryGetValue(type, out UserControl control))
				window.Content = control;
		}
	}
}
