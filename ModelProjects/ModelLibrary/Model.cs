using Common;
using System;

namespace ModelLibrary
{
    public class Model : IModel
    {
        public int RowsCount { get; }
        public int ColumnsCount { get; }
        public int LineLength { get; }

        public Model(int rowsCount, int columnsCount, int lineLength = 3)
        {
            RowsCount = rowsCount;
            ColumnsCount = columnsCount;
            LineLength = lineLength;
            Cells = new CellDto[RowsCount][];

            for (int row = 0; row < RowsCount; row++)
            {
                Cells[row] = new CellDto[ColumnsCount];
                for (int column = 0; column < ColumnsCount; column++)
                    Cells[row][column] = new CellDto(row, column, CellContent.Empty);
            }

        }

        private readonly  CellDto[][] Cells;

        public event GameOverHandler GameOverEvent;
        public event MoveHandler MoveEvent;

        public bool CanMove(CellDto cell)
            => Cells[cell.X][cell.Y].CellType == CellContent.Empty;

        public void Move(CellDto cell)
        {
            if (CanMove(cell))
            {
                Cells[cell.X][cell.Y] = cell;
                MoveEvent?.Invoke(this, cell);
                WinCheck(cell);
            }
        }

        private void WinCheck(CellDto cell)
        {
            GameOverEvent?.Invoke(this, (TestHorizontal(cell) == true || TestVertical(cell) == true || TestDiagonal(cell) == true || TestDiagonal2(cell) == true));
        }
        private bool TestLine(CellDto cell, int elementsCount, bool useShiftX, bool useShiftY, bool direction)
        {
            int count = 0;
            CellDto target;
            int shiftX = (useShiftX == true) ? 1 : 0;
            int shiftY = (useShiftY == true) ? 1 : 0;
            int directionK = (direction == true) ? 1 : -1;
            int shift = LineLength - 1;
            for (int i = -shift; i < shift; i++)
            {
                target = GetCellByPosiotion(cell.X + (directionK * shiftX * i), cell.Y + (directionK * shiftY * i));
                if (target != null && target.CellType == cell.CellType)
                {
                    count++;
                }
                else
                {
                    count = 0;
                }
            }
            if (count >= elementsCount)
            {
                return true;
            }
            return false;
        }
        private bool TestVertical(CellDto cell)//чую, что эти методы можно свести к однмоу
        {
            return TestLine(cell, 3, false, true, false);
        }
        private bool TestHorizontal(CellDto cell)
        {
            return TestLine(cell, 3, true, false, false);
        }
        private bool TestDiagonal(CellDto cell)
        {
            return TestLine(cell, 3, true, true, false);
        }
        private bool TestDiagonal2(CellDto cell)
        {
            return TestLine(cell, 3, true, true, true);
        }

        private CellDto GetCellByPosiotion(int x, int y) {
            if(x<0 || x> ColumnsCount || y<0 || y > RowsCount)
            {
                return null;
            }
            return Cells[x][y];
        }
    }
}
