using Common;
using System;
using System.Linq;

namespace ModelLibrary
{
	public class Model : IModel
	{
		public int RowsCount { get; }
		public int ColumnsCount { get; }
		public int LineLength { get; }

		
		private int ShiftForCalculateCompleteLine;

		public Model(int rowsCount, int columnsCount, int lineLength = 3)
		{
			RowsCount = rowsCount;
			ColumnsCount = columnsCount;
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

		private readonly CellDto[][] Cells;

		public event GameOverHandler GameOverEvent;
		public event MoveHandler MoveEvent;

		public bool CanMove(CellDto cell)
			=> Cells[cell.X][cell.Y].CellType == CellContent.Empty;

		public void Move(CellDto cell)
		{
			if(cell.CellType == CellContent.Empty)
			{
				throw new Exception("Произведена попытка хода пустой клеткой");
			}
			if (CanMove(cell))
			{
				Cells[cell.Y][cell.X] = cell;
				MoveEvent?.Invoke(this, cell);
				WinCheck(cell);
			}
		}

		private void WinCheck(CellDto cell)
		{
			bool isWin = (	TestHorizontal(cell) == true/* ||
							TestVertical(cell) == true ||
							TestDiagonal(cell) == true ||
							TestDiagonal2(cell) == true*/);
			if(isWin == true)
			{
				GameOverEvent?.Invoke(this);
			}
		}
		private bool TestLine(CellDto cell, int elementsCount, bool useShiftX, bool useShiftY, bool direction)
		{
			Log("start test line************************************");
			int count = 0;
			CellDto target;
			
			int shiftX = (useShiftX == true) ? 1 : 0;
			int shiftY = (useShiftY == true) ? 1 : 0;
			int directionK = (direction == true) ? -1 : 1;
			//int shift = LineLength - 1;//вынести 2 в приватное свойство
			int shiftFrom = 0;
			int shiftTo = 0;
			if (useShiftX == true)
			{
				shiftFrom = cell.X - ShiftForCalculateCompleteLine;
				shiftTo = cell.X + LineLength;
			}
			if (useShiftY == true)
			{
				shiftFrom = cell.Y - ShiftForCalculateCompleteLine;
				shiftTo = cell.Y + LineLength;
			}
			int countLines = 0;
			//Log("ft", shiftFrom, shiftTo);
			for (int i = shiftFrom; i < shiftTo; i++)
			{
				Log(shiftFrom, shiftTo, i, shiftX, shiftY);
				Log("test> ", (directionK * shiftX * i), (directionK * shiftY * i));
				//target = GetCellByPosiotion(cell.X + (directionK * shiftX * i), cell.Y + (directionK * shiftY * i));
				target = GetCellByPosiotion(directionK * shiftX * i, directionK * shiftY * i);
				if (target != null && target.CellType == cell.CellType)
				{
					Log("++");
					count++;
					if(count >= elementsCount)
					{
						countLines++;
					}
				}
				else
				{
					count = 0;
				}
			}
			Log("count lines", countLines);
			if (countLines > 0)//переделать на возврат количества линий
			{
				return true;
			}
			return false;
		}
		private bool TestVertical(CellDto cell)//чую, что эти методы можно свести к однмоу
		{
			//Console.WriteLine("vertical " + TestLine(cell, LineLength, false, true, false));
			return TestLine(cell, LineLength, false, true, false);
		}
		private bool TestHorizontal(CellDto cell)
		{
			//Console.WriteLine("horizontal " + TestLine(cell, 3, true, false, false));
			return TestLine(cell, LineLength, true, false, false);
		}
		private bool TestDiagonal(CellDto cell)
		{
			//Console.WriteLine("diagonal " + TestLine(cell, 3, true, true, false));
			return TestLine(cell, LineLength, true, true, false);
		}
		private bool TestDiagonal2(CellDto cell)
		{
			//Console.WriteLine("diagonal2 " + TestLine(cell, 3, true, true, true));
			return TestLine(cell, LineLength, true, true, true);
		}

		private CellDto GetCellByPosiotion(int x, int y) {
			if(x < 0 || x >= ColumnsCount || y < 0 || y >= RowsCount)
			{
				return null;
			}
			return Cells[x][y];
		}

		public CellDto[][] PublicCells
		{
			get {
				return Cells.Select(a => a.ToArray()).ToArray();//нашел в сети, не совем понимаю, как это работает
			}
		}

		private static void Log(params object[] args)//куда-то переместить для удобства
		{
			var res = "";
			for (int i = 0; i < args.Length - 1; i++)
			{
				res += args[i].ToString() + " ";
			}
			res += args[args.Length - 1];
			Console.WriteLine(res);
		}
	}
}
