using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesLibrary
{
	public enum CELL_CONTENT {EMPTY, CROSS, ZERO};
	public class Cell
	{
		private CELL_CONTENT _cell;
		private int _x;
		private int _y;
		public Cell(int x, int y, CELL_CONTENT cell = CELL_CONTENT.EMPTY)
		{
			_x = x;
			_y = y;
			_cell = cell;
		}
		public CELL_CONTENT CellType { get; }//пока только геттер
		public int X { get; }
		public int Y { get; }
	}
}
