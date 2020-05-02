using Common;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ViewModel
{
	public class MainViewModel : OnPropertyChangedClass, IGameEndVM, IFirstScreenVM, IGamersVM, IGameVM, ISettingsVM, IStatisticVM, IGameEndDrawVM, IStatusesVM
	{
		private readonly Action<Type> windowsChanger;
		public MainViewModel(Action<Type> windowsChanger, bool isDisignedMode = true)
		{
			this.windowsChanger = windowsChanger ?? throw new ArgumentNullException(nameof(windowsChanger));

			if (isDisignedMode)
			{
				_rowsCount = 3;
				_columnsCount = 3;
				InitCollDisign();

			}
		}

		/*public Dictionary<string, object> Pieces { get; } = new Dictionary<string, object>()//удалить?
		{
			{"crossStandrart", @"Resources/Images/cross.png" },
			{"zeroStandrart", @"Resources/Images/zero.png" },
			{"crossYes", @"Resources/Images/yes.png" },
			{"zeroNo", @"Resources/Images/no.png" }
		};*/

		//public RelayCommand ChangePieceIndexCommand { get; private set; }//удалить?

		public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

		private ICommand _showFirstScreenCommand;
		public ICommand ShowFirstScreenCommand => _showFirstScreenCommand ?? (_showFirstScreenCommand = new RelayCommand(ShowFirstScreenMethod));

		private void ShowFirstScreenMethod(object parameter)
		{
			windowsChanger(typeof(IFirstScreenVM));
		}



		private ICommand _startNewGameCommand;

		public ICommand StartNewGameCommand => _startNewGameCommand ?? (_startNewGameCommand = new RelayCommand(StartNewGameMethod, StartNewGameCanMethod));

		protected virtual bool StartNewGameCanMethod(object parameter)
		{
			if (string.IsNullOrWhiteSpace(FirstGamer.UserName) || string.IsNullOrWhiteSpace(SecondGamer.UserName))
				return false;
			if (FirstGamer.UserName == SecondGamer.UserName || FirstGamer.Image == SecondGamer.Image)
				return false;

			return true;
		}

		protected virtual void StartNewGameMethod(object parameter)
		{
			Picturies.Clear();
			Picturies.Add(CellContent.Cross, FirstGamer.Image);
			Picturies.Add(CellContent.Zero, SecondGamer.Image);
			Picturies.Add(CellContent.Empty, null);
			windowsChanger(typeof(IStatusesVM));
		}

		private IEnumerable<ImageSource> _piecesCollection;
		public IEnumerable<ImageSource> PiecesCollection { get => _piecesCollection; set => SetProperty(ref _piecesCollection, value); }
		public Gamer FirstGamer { get; } = new Gamer()
		{
			UserName = "Пользователь 1"
		};
		public Gamer SecondGamer { get; } = new Gamer()
		{
			UserName = "Пользователь 2"
		};


		private static readonly CellContent[] contens = Enum.GetValues(typeof(CellContent)).Cast<CellContent>().ToArray();
		private static readonly Random random = new Random();
		private ICommand _moveCommand;
		public ICommand MoveCommand => _moveCommand ?? (_moveCommand = new RelayCommand(MoveMethod, MoveCanMethod));
		protected virtual void MoveMethod(object p)
		{

			CellDto cell = (CellDto)p;
			Cells[cell.Row * ColumnsCount + cell.Column] = new CellDto(cell.Column, cell.Row, contens[random.Next(contens.Length - 1) + 1]);
			var clearCells = Cells.Where(c => c.CellType == CellContent.Empty).Count();
			if (clearCells == 0)
			{
				MessageBox.Show("Game over");
				//как правильно это запустить отсюда?
			}
		}
		protected virtual bool MoveCanMethod(object p)
		{
			return p is CellDto cell && cell.CellType == CellContent.Empty;
		}

		private ICommand _loseCommand;
		public ICommand LoseCommand => _loseCommand ?? (_loseCommand = new RelayCommand(LoseMethod));

		protected virtual void LoseMethod(object parameter)
		{
			//необходимо добавить изменение статистики
			windowsChanger(typeof(IGameEndVM));
		}

		private int _rowsCount;
		public int RowsCount { get => _rowsCount; protected set => SetProperty(ref _rowsCount, value); }

		private int _columnsCount;
		public int ColumnsCount { get => _columnsCount; protected set => SetProperty(ref _columnsCount, value); }
		public ObservableCollection<CellDto> Cells { get; } = new ObservableCollection<CellDto>();
		private void InitCollDisign()
		{
			Cells.Clear();
			for (int row = 0; row < RowsCount; row++)
				for (int column = 0; column < ColumnsCount; column++)
					Cells.Add(new CellDto(column, row, contens[random.Next(contens.Length)]));

			new List<User>
			{
				new User(){ Name = "Иван", Total=111,  Win=56, Lose=5 },
				new User(){ Name = "Петр", Total=31,  Win=26, Lose=5 },
				new User(){ Name = "Сидор", Total=81,  Win=44, Lose=5 },
				new User(){ Name = "Феофан", Total=1000,  Win=777, Lose=5 },
				new User(){ Name = "Акакий", Total=9999,  Win=56, Lose=888 },
				new User(){ Name = "Ivengo", Total=564,  Win=564, Lose=0 }
			}
			.ForEach(it => Users.Add(it));


		}
		private Dictionary<CellContent, ImageSource> _pictures = new Dictionary<CellContent, ImageSource>();
		public Dictionary<CellContent, ImageSource> Picturies { get => _pictures; }


		private ICommand _repairGameCommand;
		public ICommand RepairGameCommand => _repairGameCommand ?? (_repairGameCommand = new RelayCommand(StartNewGameMethod, RepairGameCanMethod));
		private bool RepairGameCanMethod(object parameter)
		{
			return File.Exists(Model.FileNameXml);
		}

		private ICommand _showSettingsCommand;
		public ICommand ShowSettingsCommand => _showSettingsCommand ?? (_showSettingsCommand = new RelayCommand(ShowSettingsMethod));

		private void ShowSettingsMethod(object parameter)
		{
			windowsChanger(typeof(ISettingsVM));
		}

		private ICommand _showStatisticCommand;
		public ICommand ShowStatisticCommand => _showStatisticCommand ?? (_showStatisticCommand = new RelayCommand(ShowStatisticMethod));
		protected virtual void ShowStatisticMethod(object parameter)
		{
			windowsChanger(typeof(IStatisticVM));
		}

		//public ICommand RevengeCommand { get; }
		private bool _isRevenge = false;
		public bool IsRevenge
		{
			get => _isRevenge;
			protected set => SetProperty(ref _isRevenge, value);
		}//добавить реализацию

		//Сначала переделал, потом прочитал твой ответ. Пока оставлю до изменения подхода.
		private Gamer _winner;//сомневаюсь, что есть смысл выносить пользователя в отдельную переменную
		public Gamer Winner
		{
			get => _winner;
			protected set => SetProperty(ref _winner, value);
		}/* = FirstGamer.Clone();*///не могу создать копию победителя и проигравшего
		private Gamer _loser;

		public Gamer Loser
		{
			get => _loser;
			protected set => SetProperty(ref _loser, value);
		}


		private GameStatuses _statuse;
		public GameStatuses Statuse
		{
			get => _statuse;
			protected set => SetProperty(ref _statuse, value);
		}

		private UserType _currentUser = UserType.Unknown;
		public UserType CurrentUser { get => _currentUser; protected set => SetProperty(ref _currentUser, value); }

		protected override void PropertyNewValue<T>(ref T fieldProperty, T newValue, string propertyName)
		{
			base.PropertyNewValue(ref fieldProperty, newValue, propertyName);
			if (propertyName == nameof(Statuse))
			{
				switch (Statuse)
				{
					case GameStatuses.Zero:
						break;
					case GameStatuses.New:
						break;
					case GameStatuses.Draw:
					case GameStatuses.Game:
						Winner = Loser = null;
						break;
					case GameStatuses.WinFirst:
						Winner = FirstGamer;
						Loser = SecondGamer;
						break;
					case GameStatuses.WinSecond:
						Winner = SecondGamer;
						Loser = FirstGamer;
						break;
					case GameStatuses.Cancel:
						break;
					default:
						break;
				}
			}

			if (propertyName == nameof(IsRevenge))
				if (RepairGameCommand is RelayCommand relayCommand)
					relayCommand.Invalidate();
				else
					OnPropertyChanged(nameof(RepairGameCommand));
		}
	}
}
