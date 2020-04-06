using Common;
using System;
using Utils;
using System.Linq;
using System.Collections.ObjectModel;

namespace ModelLibrary
{
	public class Model : IModel
	{
		public int RowsCount { get; }
		public int ColumnsCount { get; }
		public int LineLength { get; }


		private readonly int ShiftForCalculateCompleteLine;//сдвиг относительно проверяемой ячейки
		private int TotalFreeCells;

		public event GameOverHandler GameOverWinEvent;
		public event GameOverDrawHandler GameOverDrawEvent;
		public event MoveHandler MoveEvent;

		private readonly ReadOnlyCollection<CellDto[]> cells;
		public ReadOnlyCollection<ReadOnlyCollection<CellDto>> Cells { get; }
		//{
		//	get {
		//		return Cells.Select(a => a.ToArray()).ToArray();//нашел в сети, не совем понимаю, как это работает
		//	}
		//}

		public Model(int rowsCount, int columnsCount, int lineLength = 3)
		{
			RowsCount = rowsCount;
			ColumnsCount = columnsCount;
			TotalFreeCells = RowsCount * ColumnsCount;
			LineLength = lineLength;
			ShiftForCalculateCompleteLine = LineLength - 1;

			// Делаем временный массив и заполняем его
			var cells = new CellDto[RowsCount][];

			for (int row = 0; row < RowsCount; row++)
			{
				cells[row] = new CellDto[ColumnsCount];
				for (int column = 0; column < ColumnsCount; column++)
					cells[row][column] = new CellDto(row, column, CellContent.Empty);
			}

			/// Так как строки не меняются, то преобразуем 
			/// временный массив в неизменяемый по первому измерению
			this.cells = Array.AsReadOnly(cells);

			/// Публичный массив делаем неизменяемым по двум измерениям.
			/// При этом у нас сохраняется связь с элементами внутреннего массива,
			/// так Array.AsReadOnly(_row)) делает не копию, а оболочку надо 
			/// массивом каждой строки. И изменяя значения в исходном массиве мы их меняем 
			/// и в публичном. Но "из вне" эти значения поменять не могут.
			Cells = Array.AsReadOnly(cells.Select(_row => Array.AsReadOnly(_row)).ToArray());
		}



		public bool CanMove(CellDto cell)
			=> cells[cell.Y][cell.X].CellType == CellContent.Empty;

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
				cells[cell.Y][cell.X] = cell;
				MoveEvent?.Invoke(this, cell);
				bool isWin = WinCheck(cell);
				if (isWin == false && TotalFreeCells == 0)
				{
					GameOverDrawEvent?.Invoke(this);
				}
			}
		}

		private bool WinCheck(CellDto cell)
		{
			bool isWin = (
				TestLine(cell, LineLength, false, true, false) ||   //vertical
				TestLine(cell, LineLength, true, false, false) ||   //horizontal
				TestLine(cell, LineLength, true, true, false) ||	//diagonal-1
				TestLine(cell, LineLength, true, true, true)		//diagonal-2
				);

			if (isWin == true)
			{
				GameOverWinEvent?.Invoke(this);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Проверка на появление новой завершенной линии относительно свежедобавленного элемента
		/// </summary>
		/// <param name="cell">ячейка, относительно которой осуществляется проверка</param>
		/// <param name="elementsCount">количество подряд идущих однотипных элементов в ряду, необходимых для зачисления новой линии</param>
		/// <param name="useShiftX">использование при проверке сдвига по оси X</param>
		/// <param name="useShiftY">использование при проверке сдвига по оси Y</param>
		/// <param name="directionForDiagonalTest">должно быть true при проверке диагонали, идущей с верхнего правого конца относительно выбранной ячейки к левому нижнему концу</param>
		/// <returns>Возвращает true в случае появления хоть одной новой заполненной линии</returns>
		private bool TestLine(CellDto cell, int elementsCount, bool useShiftX, bool useShiftY, bool directionForDiagonalTest)
		{
			//Utils.Utils.Log("start test line************************************", useShiftX, useShiftY, directionForDiagonalTest);
			int shiftFromX = 0;//точка начала проверки по оси X
			int shiftFromY = (useShiftY == true) ? cell.Y - ShiftForCalculateCompleteLine : 0;//точка начала проверки по оси Y - вычисляется сразу т.к. параметр directionForDiagonalTest не влияет на рассчеты по оси Y
			int diagonalFactor = 1;//коэффициент для рассчета в случае проверки совпадений по 2й диагонали
			int countCoinCidencesInLine = 0;//количество совпадений в линии

			int countLinesComplete = 0;
			if (useShiftX == true)//смещение по оси X
			{
				if (directionForDiagonalTest == true)//рассчет по диагонали с правой стороны
				{
					shiftFromX = cell.X + ShiftForCalculateCompleteLine;
					diagonalFactor = -1;
				}
				else
				{
					shiftFromX = cell.X - ShiftForCalculateCompleteLine;
				}
			}
			int length = ShiftForCalculateCompleteLine * 2 + 1;//количество определяемых ячеек - длина возможной линии в обе стороны (с учетом текущей ячейки)

			for (int i = 0; i < length; i++)
			{
				int x = (useShiftX) ? shiftFromX + (diagonalFactor * i) : cell.X;//координаты проверяемой ячейки по оси X
				int y = (useShiftY) ? shiftFromY + i : cell.Y;//координаты проверяемой ячейки по оси Y
				CellDto targetCell = GetCellByPosiotion(x, y);
				//Utils.Utils.Log("test cell (x, y):", x, y);//не могу понять, почему не получается сокращенный вариант
				if (targetCell != null && targetCell.CellType == cell.CellType)//если ячейка существует и типы совпадают...
				{
					countCoinCidencesInLine++;
					if (countCoinCidencesInLine >= LineLength)
					{
						countLinesComplete++;
					}
				}
				else
				{
					countCoinCidencesInLine = 0;
				}
			}
			return countLinesComplete > 0;
		}

		private CellDto GetCellByPosiotion(int x, int y)
		{
			if (x < 0 || x >= ColumnsCount || y < 0 || y >= RowsCount)
			{
				return null;
			}
			return cells[y][x];
		}

	}
}
