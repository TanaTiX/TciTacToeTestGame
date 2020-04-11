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

		public CellDto[][] Cells
		{
			get
			{
				var cells = new CellDto[RowsCount][];

				for (int row = 0; row < RowsCount; row++)
				{
					cells[row] = new CellDto[ColumnsCount];
					for (int column = 0; column < ColumnsCount; column++)
						cells[row][column] = new CellDto(row, column, CellContent.Empty);
				}
				return cells;
			}
		}
	}
}
