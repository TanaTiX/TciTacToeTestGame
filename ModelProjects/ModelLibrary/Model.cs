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
        }
    }
}
