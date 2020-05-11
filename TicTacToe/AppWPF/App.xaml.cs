using Common;
using LibVM;
using Model;
using ModelLibrary;
using Repo;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using View;
using ViewModel;

namespace GameWPF
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		private Model.ModelTicTacToe model;
		/// <summary>Экземпляр главного окна</summary>
		/// <remarks>Экземпляр создаётся один раз на всё время жизни приложения.
		/// При закрытии окна происходит закрытие приложения.</remarks>
		private readonly Window window = new Window()
		{
			WindowStartupLocation = WindowStartupLocation.Manual,
			ResizeMode = ResizeMode.NoResize,
			Left = 1320,
			Top = 40
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
		private readonly GameTotalUC gameUC = new GameTotalUC();

		/// <summary>Экземпляр экрана результата игры - есть победитель</summary>
		/// <remarks>Экземпляр создаётся один раз на всё время жизни приложения</remarks>
		//private readonly GameEndUC gameEndUC = new GameEndUC();
		/// <summary>Экземпляр экрана результата игры - ничья</summary>
		/// <remarks>Экземпляр создаётся один раз на всё время жизни приложения</remarks>
		//private readonly DrawUC drawUC = new DrawUC();



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
			controls.Add(typeof(IStatusesVM), gameUC);

			//controls.Add(typeof(IGameEndVM), gameEndUC);
			//controls.Add(typeof(IGameEndDrawVM), drawUC);

			
			window.Width = 600;
			window.Height = 700;
			ChangeWindowContent(typeof(IFirstScreenVM));

			ReposSaveGameXML repos = new ReposSaveGameXML("SavedGame.xml");

			//Model.ModelTicTacToe.FileNameXml = ;
			model = new ModelTicTacToe(repos);
			//MainViewModel viewModel = new MainViewModel(ChangeWindowContent);
			MainVM viewModel = new MainVM(ChangeWindowContent, model , 3, 3, 3);
			viewModel.CellTypes.Add(new CellTypeDto(0, "Empty"));
			viewModel.CellTypes.Add(new CellTypeDto(1, "Cross"));
			viewModel.CellTypes.Add(new CellTypeDto(2, "Zero"));
			viewModel.FirstGamer.CellType = viewModel.CellTypes[1];
			viewModel.SecondGamer.CellType = viewModel.CellTypes[2];
			ImageSource[] images =
			{
				(ImageSource)imageSourceConverter.ConvertFrom(new Uri("pack://application:,,,/View;component/Resources/Images/cross.png")),
				(ImageSource)imageSourceConverter.ConvertFrom(new Uri("pack://application:,,,/View;component/Resources/Images/zero.png")),
				(ImageSource)imageSourceConverter.ConvertFrom(new Uri("pack://application:,,,/View;component/Resources/Images/yes.png")),
				(ImageSource)imageSourceConverter.ConvertFrom(new Uri("pack://application:,,,/View;component/Resources/Images/no.png"))
			};

			viewModel.FirstGamer.Image = images[0];
			viewModel.SecondGamer.Image = images[1];
			viewModel.FirstGamer.ImageIndex = 0;
			viewModel.SecondGamer.ImageIndex = 1;
			viewModel.PiecesCollection = images;
			//viewModel.IsRevenge = File.Exists(Model.FileNameXml);

			window.DataContext = viewModel;
			window.Show();
		}

		public static readonly ImageSourceConverter imageSourceConverter = new ImageSourceConverter();

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

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);
			model.Save();
		}
	}
}
