namespace Common
{
	public class CellDto
	{
		public CellDto(int x, int y, CellContent cell = CellContent.Empty)
		{
			X = x;
			Y = y;
			CellType = cell;
		}
		public CellContent CellType { get; }//пока только геттер
		public int X { get; }
		public int Y { get; }
	}
}
