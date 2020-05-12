using Common;
using LibVM;
using Model;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

		public ObservableCollection<UserStatistic> Users { get; } = new ObservableCollection<UserStatistic>();

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

		private void UpdatePictures()
		{
			Picturies.Clear();
			Picturies.Add(FirstGamer.CellType, FirstGamer.Image);
			Picturies.Add(SecondGamer.CellType, SecondGamer.Image);
			Picturies.Add(CellTypes[0], null);
		}
		protected virtual void StartNewGameMethod(object parameter)
		{
			UpdatePictures();

			windowsChanger(typeof(IStatusesVM));
		}

		private IEnumerable<ImageSource> _piecesCollection;
		public IEnumerable<ImageSource> PiecesCollection { get => _piecesCollection; set => SetProperty(ref _piecesCollection, value); }
		public UserVM FirstGamer { get; } = new UserVM()
		{
			UserName = "Пользователь 1",
			Id = 1,
			Turn=7
		};
		public UserVM SecondGamer { get; } = new UserVM()
		{
			UserName = "Пользователь 2",
			Id=2,
			Turn=15
		};


		//private static readonly CellContent[] contens = Enum.GetValues(typeof(CellContent)).Cast<CellContent>().ToArray();
		public ObservableCollection<CellTypeDto> CellTypes { get; } = new ObservableCollection<CellTypeDto>();

		//public MainViewModel()
		//{
		//	CellTypes.CollectionChanged += CellTypes_CollectionChanged;
		//}

		//private void CellTypes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		//{
		//	if (CellTypes.Count == 0 || CellTypes[0] != CellTypeDto.Empty)
		//		CellTypes.Insert(0, CellTypeDto.Empty);
		//	if (e.Action == NotifyCollectionChangedAction.Add)
		//	{
		//		bool rem = false;
		//		foreach (CellTypeDto cell in e.NewItems.Cast<CellTypeDto>().Where(it => string.Equals(it.Type, CellTypeDto.Empty.Type, StringComparison.OrdinalIgnoreCase)))
		//		{
		//			if (cell != CellTypes[0])
		//			rem = true;
		//			CellTypes.Remove(cell);
		//		}
		//	}
		//}

		private static readonly Random random = new Random();
		private ICommand _moveCommand;
		public ICommand MoveCommand => _moveCommand ?? (_moveCommand = new RelayCommand(MoveMethod, MoveCanMethod));
		protected virtual void MoveMethod(object p)
		{

			CellVM cell = (CellVM)p;
			//Cells[cell.Row * ColumnsCount + cell.Column] = new CellDto(cell.Column, cell.Row, CellTypes[random.Next(CellTypes.Count - 1) + 1]);
			cell.CellType = CellTypes[random.Next(CellTypes.Count - 1) + 1];
			var clearCells = Cells.Count(c => c.CellType == null);
			if (clearCells == 0)
			{
				MessageBox.Show("Game over");
				//как правильно это запустить отсюда?
			}
		}
		protected virtual bool MoveCanMethod(object p)
		{
			return p is CellVM cell && cell.CellType == null;
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
		public ObservableCollection<CellVM> Cells { get; } = new ObservableCollection<CellVM>();

		protected CellVM[,] cellsMatrix;
		protected void InitCollDisign()
		{
			Cells.Clear();
			cellsMatrix = new CellVM[RowsCount, ColumnsCount];
			for (int row = 0; row < RowsCount; row++)
				for (int column = 0; column < ColumnsCount; column++)
					Cells.Add(cellsMatrix[row, column] = new CellVM()
					{
						Column = column,
						Row = row,
						CellType = CellTypes[random.Next(CellTypes.Count - 1) + 1]
					});

			new List<UserStatistic>
			{
				new UserStatistic(){ Name = "Иван", Total=111,  Win=56, Lose=5 },
				new UserStatistic(){ Name = "Петр", Total=31,  Win=26, Lose=5 },
				new UserStatistic(){ Name = "Сидор", Total=81,  Win=44, Lose=5 },
				new UserStatistic(){ Name = "Феофан", Total=1000,  Win=777, Lose=5 },
				new UserStatistic(){ Name = "Акакий", Total=9999,  Win=56, Lose=888 },
				new UserStatistic(){ Name = "Ivengo", Total=564,  Win=564, Lose=0 }
			}
			.ForEach(it => Users.Add(it));


		}
		public Dictionary<CellTypeDto, ImageSource> Picturies { get ; } = new Dictionary<CellTypeDto, ImageSource>();


		private ICommand _repairGameCommand;
		public ICommand RepairGameCommand => _repairGameCommand ?? (_repairGameCommand = new RelayCommand(RepairGameMethod, RepairGameCanMethod));

		protected virtual void RepairGameMethod(object parameter)
		{
			UpdatePictures();

			windowsChanger(typeof(IStatusesVM));
		}

		private bool RepairGameCanMethod(object parameter)
		{
			return IsRevenge;
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
		private UserVM _winner;//сомневаюсь, что есть смысл выносить пользователя в отдельную переменную
		public UserVM Winner
		{
			get => _winner;
			protected set => SetProperty(ref _winner, value);
		}/* = FirstGamer.Clone();*///не могу создать копию победителя и проигравшего
		private UserVM _loser;

		public UserVM Loser
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

		private int _currentUserIndex = -1;
		public int CurrentUserIndex { get => _currentUserIndex; protected set => SetProperty(ref _currentUserIndex, value); }

		public UserVM CurrentUser => CurrentUserIndex == 0 ? FirstGamer
			: CurrentUserIndex == 1 ? SecondGamer
			: null;

		private int _lineLength;
		public int LineLength { get => _lineLength; protected set => SetProperty(ref _lineLength, value); }

		protected override void PropertyNewValue<T>(ref T fieldProperty, T newValue, string propertyName)
		{
			base.PropertyNewValue(ref fieldProperty, newValue, propertyName);
			if (propertyName == nameof(CurrentUserIndex))
			{
				OnPropertyChanged(nameof(CurrentUser));
				// Это логика МОДЕЛИ !!!!
				//if (CurrentUserIndex == 0)
				//{
				//	SecondGamer.IsTurn = !(FirstGamer.IsTurn = true);
				//}
				//else
				//{
				//	SecondGamer.IsTurn = !(FirstGamer.IsTurn = false);
				//}
			}

			if (propertyName == nameof(IsRevenge))
				if (RepairGameCommand is RelayCommand relayCommand)
					relayCommand.Invalidate();
				else
					OnPropertyChanged(nameof(RepairGameCommand));
		}
	}
}
