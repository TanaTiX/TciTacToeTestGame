using Common;
using System;

namespace ModelLibrary
{
    public class Model : IModel
    {
        public int RowsCount { get; }
        public int ColumnsCount { get; }

        public Model(int rowsCount, int columnsCount)
        {
            RowsCount = rowsCount;
            ColumnsCount = columnsCount;
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
            // Здесь надо проверить выигрыш или проирыш
            // и если есть, то создать событие
            // GameOverEvent?.Invoke(this, true или false);
            GameOverEvent?.Invoke(this, (TestHorizontal(cell) == true || TestVertical(cell) == true || TestDiagonal(cell) == true));
        }

        private bool TestVertical(CellDto cell)//чую, что эти методы можно свести к однмоу
        {
            int count = 0;
            CellDto target;

            for (int i = -2; i < 2; i++)//цифровые значения лучше поместить в константы?
            {
                target = GetCellByPosiotion(cell.X, cell.Y + i);
                if (target != null && target.CellType == cell.CellType)
                {
                    count++;
                }
            }
            if (count >= 3)
            {
                return true;
            }
            return false;
        }
        private bool TestHorizontal(CellDto cell)
        {
            int count = 0;
            CellDto target;
            
            for (int i = -2; i < 2; i++)//цифровые значения лучше поместить в константы?
            {
                target = GetCellByPosiotion(cell.X + i, cell.Y);
                if(target!=null && target.CellType == cell.CellType)
                {
                    count++;
                }
            }
            if (count >= 3)
            {
                return true;
            }
            return false;
        }
        private bool TestDiagonal(CellDto cell)
        {
            int count = 0;
            CellDto target;

            for (int i = -2; i < 2; i++)//цифровые значения лучше поместить в константы?
            {
                target = GetCellByPosiotion(cell.X + i, cell.Y + i);
                if (target != null && target.CellType == cell.CellType)
                {
                    count++;
                }
            }
            if (count >= 3)
            {
                return true;
            }
            return false;
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
