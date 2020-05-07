using Common;
using System;

namespace ModelLibrary
{
	public class CellDto
	{
		private static Random rnd = new Random();
		public CellDto( int row,int column, CellTypeDto cellType)
		{
			Name = "test " + rnd.Next();
			Column = column;
			Row = row;
			CellType = cellType;
		}
		public CellDto(int id, int row, int column, CellTypeDto cellType)
			: this(column,  row, cellType)
		{
			Id = id;
		}
		public CellTypeDto CellType { get; }
		public int Column { get; }
		public int Row { get; }
		public int Id { get; }
		public string Name { get; }
	}
}
