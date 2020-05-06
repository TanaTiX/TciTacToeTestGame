using Common;
using System;

namespace ModelLibrary
{
	public class CellDto
	{
		private static Random rnd = new Random();
		public CellDto( int row,int column, int cellTypeId)
		{
			Name = "test " + rnd.Next();
			Column = column;
			Row = row;
			CellType = cellTypeId;
		}
		public CellDto(int id, int row, int column, int cellTypeId)
			: this(column,  row, cellTypeId)
		{
			Id = id;
		}
		public int CellType { get; }
		public int Column { get; }
		public int Row { get; }
		public int Id { get; }
		public string Name { get; }
	}
}
