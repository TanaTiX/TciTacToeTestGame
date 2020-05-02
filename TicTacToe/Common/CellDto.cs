using System;

namespace Common
{
	public class CellDto
	{
		private static Random rnd = new Random();
		public CellDto(int column, int row, CellContent cell = CellContent.Empty)
		{
			Name = "test " + rnd.Next();
			Column = column;
			Row = row;
			CellType = cell;
		}
		public CellContent CellType { get; }//пока только геттер
		public int Column { get; }
		public int Row { get; }
		public string Name { get; }
	}
}
