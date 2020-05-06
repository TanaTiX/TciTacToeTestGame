using System.Collections.Generic;

namespace ModelLibrary
{
	public partial class SavedGameDto
	{
		public SavedGameDto(ISet<UserDto> users, ISet<CellDto> cells, ISet<CellTypeDto> types,int rowsCount, int columnsCount,  int lengthLineForWin)
		{
			Users = users;
			Cells = cells;
			Types = types;
			ColumnsCount = columnsCount;
			RowsCount = rowsCount;
			LengthLineForWin = lengthLineForWin;
		}

		public ISet<UserDto> Users { get; }

		public ISet<CellDto> Cells { get; }

		public ISet<CellTypeDto> Types { get; }

		public int ColumnsCount { get; }
		public int RowsCount { get; }
		public int LengthLineForWin { get; }
	}


}
