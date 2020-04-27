using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ViewModel
{
	public class MainViewModel : OnPropertyChangedClass, IGameEndVM, IFirstScreenVM, IGamersVM, IGameVM, ISettingsVM, IStatisticVM
	{
		private readonly Action<Type> windowsChanger;
		public MainViewModel(Action<Type> windowsChanger, bool isDisignedMode = true)
		{
			this.windowsChanger = windowsChanger ?? throw new ArgumentNullException(nameof(windowsChanger));

			if (isDisignedMode)
			{
				RowsCount = 3;
				ColumnsCount = 3;
				ClearGameCells();
			}
		}
        
		public Dictionary<string, object> Pieces { get; } = new Dictionary<string, object>()//удалить?
		{
			{"crossStandrart", @"Resources/Images/cross.png" },
			{"zeroStandrart", @"Resources/Images/zero.png" },
			{"crossYes", @"Resources/Images/yes.png" },
			{"zeroNo", @"Resources/Images/no.png" }
		};

		public RelayCommand ChangePieceIndexCommand { get; private set; }//удалить?

		public ObservableCollection<User> Users {get;}

		private ICommand _showFirstScreenCommand;
		public ICommand ShowFirstScreenCommand => _showFirstScreenCommand ?? (_showFirstScreenCommand = new RelayCommand(ShowFirstScreenMethod));

		private void ShowFirstScreenMethod(object parameter)
		{
			windowsChanger(typeof(IFirstScreenVM));
		}

		

		private ICommand _startNewGameCommand;

		public ICommand StartNewGameCommand => _startNewGameCommand ?? (_startNewGameCommand = new RelayCommand(StartNewGameMethod, StartNewGameCanMethod));

		private bool StartNewGameCanMethod(object parameter)
		{
			if (string.IsNullOrWhiteSpace(FirstGamer.UserName) || string.IsNullOrWhiteSpace(SecondGamer.UserName))
				return false;
			if (FirstGamer.UserName == SecondGamer.UserName || FirstGamer.Image == SecondGamer.Image)
				return false;

			return true;
		}

		private void StartNewGameMethod(object parameter)
		{
			Picturies.Clear();
			Picturies.Add(CellContent.Cross, FirstGamer.Image);
			Picturies.Add(CellContent.Zero, SecondGamer.Image);
			Picturies.Add(CellContent.Empty, null);
			windowsChanger(typeof(IGameVM));
		}

		public IEnumerable<ImageSource> PiecesCollection { get; set; }

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
			Cells[cell.X * ColumnsCount + cell.Y] = new CellDto(cell.X, cell.Y, contens[random.Next(contens.Length - 1) + 1]);
			var clearCells = Cells.Where(c => c.CellType == CellContent.Empty).Count();
			if(clearCells == 0)
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

		private void LoseMethod(object parameter)
		{
			//необходимо добавить изменение статистики
			windowsChanger(typeof(IGameEndVM));
		}

		public int RowsCount { get; }

		public int ColumnsCount { get; }

		public ObservableCollection<CellDto> Cells { get; } = new ObservableCollection<CellDto>();
		private void ClearGameCells()
		{
			for (int row = 0; row < RowsCount; row++)
				for (int column = 0; column < ColumnsCount; column++)
					Cells.Add(new CellDto(row, column, contens[random.Next(contens.Length)]));
		}
		public Dictionary<CellContent, ImageSource> Picturies { get; } = new Dictionary<CellContent, ImageSource>();


		private ICommand _repairGameCommand;
		public ICommand RepairGameCommand => _repairGameCommand ?? (_repairGameCommand = new RelayCommand(StartNewGameMethod));
		private bool RepairGameCanMethod(object parameter)
		{
			return true;//добавить реализацию
		}

		private ICommand _showSettingsCommand;
		public ICommand ShowSettingsCommand => _showSettingsCommand ?? (_showSettingsCommand = new RelayCommand(ShowSettingsMethod));

		private void ShowSettingsMethod(object parameter)
		{
			windowsChanger(typeof(ISettingsVM));
		}

		private ICommand _showStatisticCommand;
		public ICommand ShowStatisticCommand => _showStatisticCommand ?? (_showStatisticCommand = new RelayCommand(ShowStatisticMethod));
		private void ShowStatisticMethod(object parameter)
		{
			windowsChanger(typeof(IStatisticVM));
		}

		public bool IsRevenge { get; }

		public ICommand RevengeCommand { get; }

		public Gamer Winner { get; set; }/* = FirstGamer.Clone();*///не могу создать копию победителя и проигравшего

		public Gamer Loser { get; }
	}
}
