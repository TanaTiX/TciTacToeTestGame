using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace View
{
	public class GameVM : IGameVM
	{
		public ICommand MoveCommand { get; }

		public ICommand LoseCommand { get; }

		public int RowsCount => 3;

		public int ColumnsCount => 3;

		public GameVM()
		{
			Data2D = data2D;
		}

		string[] data2D = { "Resources/Images/cross.png", "true", "false" , "true", "true", "false", "true", "true", "false" };
		

		private string[] _data2D;
		public string[] Data2D
		{
			get { return _data2D; }
			set { _data2D = value; }
		}

		public string[][] TestArray
		{
			get
			{
				var cells = new string[RowsCount][];
				for (int i = 0; i < RowsCount; i++)
				{
					cells[i] = new string[ColumnsCount];
					for (int column = 0; column < ColumnsCount; column++)
						cells[i][column] = "string" + i + " " + column;
				}
				return cells;
			}
		}

		public CellDto[] Cells
		{
			get
			{
				CellDto[] cells = new CellDto[RowsCount * ColumnsCount];

				for (int i = 0; i < RowsCount * ColumnsCount; i++)
				{
					cells[i] = new CellDto(i, i % ColumnsCount, CellContent.Empty);
				}
				return cells;
			}
		}
	}
}
