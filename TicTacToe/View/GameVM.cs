using Common;
using LibVM;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace View
{
	public class GameVM : IGameVM
	{
		public ICommand MoveCommand { get; }

		public ICommand LoseCommand { get; }

		public int RowsCount => 3;

		public int ColumnsCount => 3;



		public Dictionary<CellTypeDto, ImageSource> Picturies { get; }
			= new Dictionary<CellTypeDto, ImageSource>();


		public ObservableCollection<CellVM> Cells { get; }
			= new ObservableCollection<CellVM>();

		public UserVM FirstGamer { get; set; }

		public UserVM SecondGamer { get; set; }

		private static readonly Random random = new Random();
		public GameVM()
		{

			for (int row = 0; row < RowsCount; row++)
				for (int column = 0; column < ColumnsCount; column++)
					Cells.Add(new CellVM() {Row= row,Column= column,CellType= CellTypes[random.Next(CellTypes.Count)] });

			MoveCommand = new RelayCommand
			(
				p =>
				{
					CellDto cell = (CellDto)p;
					Cells[cell.Row * ColumnsCount + cell.Column].CellType =  CellTypes[random.Next(CellTypes.Count - 1) + 1];
				},
				p => p is CellDto cell && (string.IsNullOrWhiteSpace(cell.CellType.Type) || cell.CellType.Type == "Empty")

			);
		}
		
		public UserVM CurrentUser { get; set; }

		public int LineLength { get; set; }

		public ObservableCollection<CellTypeDto> CellTypes { get; }
			= new ObservableCollection<CellTypeDto>(){ new CellTypeDto(1,"Empty"), new CellTypeDto(2,"Cross"), new CellTypeDto(3,"Zero")};

		public int CurrentUserIndex { get; set; }
	}
}
