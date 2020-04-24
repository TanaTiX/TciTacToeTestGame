using Common;
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

		//public GameVM()
		//{
		//	Data2D = data2D;
		//}

		//string[] data2D = { "Resources/Images/cross.png", "true", "false" , "true", "true", "false", "true", "true", "false" };


		//private string[] _data2D;
		//public string[] Data2D
		//{
		//	get { return _data2D; }
		//	set { _data2D = value; }
		//}

		//public string[][] TestArray
		//{
		//	get
		//	{
		//		var cells = new string[RowsCount][];
		//		for (int i = 0; i < RowsCount; i++)
		//		{
		//			cells[i] = new string[ColumnsCount];
		//			for (int column = 0; column < ColumnsCount; column++)
		//				cells[i][column] = "string" + i + " " + column;
		//		}
		//		return cells;
		//	}
		//}

		public Dictionary<CellContent, ImageSource> Picturies { get; }
			= new Dictionary<CellContent, ImageSource>();


		public ObservableCollection<CellDto> Cells { get; }
			= new ObservableCollection<CellDto>();

		private static readonly CellContent[] contens = Enum.GetValues(typeof(CellContent)).Cast<CellContent>().ToArray();
		private static readonly Random random = new Random();
		public GameVM()
		{

			for (int row = 0; row < RowsCount; row++)
				for (int column = 0; column < ColumnsCount; column++)
					Cells.Add(new CellDto(row, column, contens[random.Next(contens.Length)]));

			MoveCommand = new RelayCommand
			(
				p =>
				{
					CellDto cell = (CellDto)p;
					Cells[cell.X * ColumnsCount + cell.Y] = new CellDto(cell.X, cell.Y, contens[random.Next(contens.Length - 1) + 1]);
				},
				p => p is CellDto cell && cell.CellType == CellContent.Empty

			);
		}
	}
}
