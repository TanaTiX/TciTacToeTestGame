namespace Common
{
    public class CellDto
    {
        //private CellContent _cell;
        //private int _x;
        //private int _y;
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
