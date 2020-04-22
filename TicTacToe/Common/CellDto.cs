using System;

namespace Common
{
	public class CellDto
	{
		private static Random rnd = new Random();
		public CellDto(int x, int y, CellContent cell = CellContent.Empty)
		{
			Name = "test " + rnd.Next();
			X = x;
			Y = y;
			CellType = cell;
		}
		public CellContent CellType { get; }//пока только геттер
		public int X { get; }
		public int Y { get; }
		public string Name { get; }
	}
}
