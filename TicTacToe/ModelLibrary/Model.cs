using Common;
using System;
using Utils;
using System.Linq;

namespace ModelLibrary
{
	public class Model : IModel
	{
		public int RowsCount { get; }
		public int ColumnsCount { get; }
		public int LineLength { get; }

		
		private readonly int ShiftForCalculateCompleteLine;//сдвиг относительно проверяемой ячейки
		private int TotalFreeCells;

		private readonly CellDto[][] Cells;

		public event GameOverHandler GameOverWinEvent;
		public event GameOverDrawHandler GameOverDrawEvent;
		public event MoveHandler MoveEvent;

		public Model(int rowsCount, int columnsCount, int lineLength = 3)
		{
			RowsCount = rowsCount;
			ColumnsCount = columnsCount;
			TotalFreeCells = RowsCount * ColumnsCount;
			LineLength = lineLength;
			ShiftForCalculateCompleteLine = LineLength - 1;
			Cells = new CellDto[RowsCount][];

			for (int row = 0; row < RowsCount; row++)
			{
				Cells[row] = new CellDto[ColumnsCount];
				for (int column = 0; column < ColumnsCount; column++)
					Cells[row][column] = new CellDto(row, column, CellContent.Empty);
			}

		}

		

		public bool CanMove(CellDto cell)
			=> Cells[cell.Y][cell.X].CellType == CellContent.Empty;

		public void Move(CellDto cell)
		{
			//Utils.Utils.Log("x", cell.X, "y", cell.Y);
			if (cell.CellType == CellContent.Empty)
			{
				throw new Exception("Произведена попытка хода пустой клеткой");
			}
			if (CanMove(cell))
			{
				TotalFreeCells--;
				Cells[cell.Y][cell.X] = cell;
				MoveEvent?.Invoke(this, cell);
				bool isWin = WinCheck(cell);
				if(isWin == false && TotalFreeCells == 0)
				{
					GameOverDrawEvent?.Invoke(this);
				}
			}
		}

		private bool WinCheck(CellDto cell)
		{
			bool isWin = (
				TestLine(cell, LineLength,	false,	true,	false)	||	//vertical
				TestLine(cell, LineLength,	true,	false,	false)	||	//horizontal
				TestLine(cell, LineLength,	true,	true,	false)	||	//diagonal-1
				TestLine(cell, LineLength,	true,	true,	true)		//diagonal-2
				);
			
			if(isWin == true)
			{
				GameOverWinEvent?.Invoke(this);
				return true;
			}
			return false;
		}
		private bool TestLine(CellDto cell, int elementsCount, bool useShiftX, bool useShiftY, bool direction)
		{
			//Utils.Utils.Log("start test line************************************", useShiftX, useShiftY, direction);
			int shiftFromX = 0;//точка начала проверки по оси X
			int shiftFromY = 0;//точка начала проверки по оси Y
			int k = 1;//коэффициент для рассчета в случае проверки совпадений по 2й диагонали
			int countCoinCidencesInLine = 0;//количество совпадений в линии
			
			int countLinesComplete = 0;
			if (useShiftX == true)//смещение по оси X
			{
				if (direction == true)//рассчет по диагонали с правой стороны
				{
					shiftFromX = cell.X + ShiftForCalculateCompleteLine;
					k = -1;
				}
				else
				{
					shiftFromX = cell.X - ShiftForCalculateCompleteLine;
				}
			}
			if(useShiftY == true)
			{
				shiftFromY = cell.Y - ShiftForCalculateCompleteLine;
			}
			int length = ShiftForCalculateCompleteLine * 2 + 1;

			CellDto targetCell;
			int x;
			int y;
			for (int i = 0; i < length; i++)
			{
				x = (useShiftX) ? shiftFromX + (k * i) : cell.X;
				y = (useShiftY) ? shiftFromY + i : cell.Y;
				targetCell = GetCellByPosiotion(x, y);
				Utils.Utils.Log("test cell (x, y):", x, y);//не могу понять, почему не получается сокращенный вариант
				if (targetCell != null && targetCell.CellType == cell.CellType)
				{
					countCoinCidencesInLine++;
					if(countCoinCidencesInLine >= LineLength)
					{
						countLinesComplete++;
					}
				}else
				{
					countCoinCidencesInLine = 0;
				}
			}
			if (countLinesComplete > 0)
			{
				return true;
			}
			return false;
		}
		
		private CellDto GetCellByPosiotion(int x, int y) {
			if(x < 0 || x >= ColumnsCount || y < 0 || y >= RowsCount)
			{
				return null;
			}
			return Cells[y][x];
		}

		public CellDto[][] PublicCells
		{
			get {
				return Cells.Select(a => a.ToArray()).ToArray();//нашел в сети, не совем понимаю, как это работает
			}
		}

		
	}
}
