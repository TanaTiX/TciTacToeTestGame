using Common;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;
using ModelLibrary;
using Repo;
using System.Collections.Generic;
using System.CodeDom;

namespace Model
{

	



	public class ModelTicTacToe :  IModel
	{
		protected int RowsCount;
		protected int ColumnsCount;
		protected int LineLength;
		protected GameStatuses GameStatus;
		protected UserDto[] Gamers;
		protected ISet<CellTypeDto> Types;
		protected int CurrentGamerId => CurrentGamer.Id;
		protected int CurrentGamerIndex = -1;
		protected UserDto CurrentGamer => Gamers[CurrentGamerIndex];


		//private int ShiftForCalculateCompleteLine;//сдвиг относительно проверяемой ячейки
		//public event NotifyChangedCellHandler ChangedCellEvent;

		void SetCellType(CellDto cell, CellTypeDto type)
		{
			if ( Cells[cell.Row, cell.Column] != null && Cells[cell.Row, cell.Column].CellType == type)
				return;
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.CellType,
				Cells[cell.Row, cell.Column] = new CellDto(Cells[cell.Row, cell.Column]?.Id ?? cell.Id, cell.Row, cell.Column, type)));
		}
		void SetCellType(CellDto cell)
			=> SetCellType(cell, cell.CellType);

		public event NotifyChangedStateHandler ChangedStateEvent;

		void SetIsRevenge(bool value)
		{
			if (IsRevenge == value)
				return;
			IsRevenge = value;
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.IsRevenge, value));
		}
		void ChangeCellsCount(int rows, int columns)
		{
			if (Cells == null || Cells.GetLength(0) != rows || Cells.GetLength(1) != columns)
			{
				Cells = new CellDto[rows, columns];
				ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.ChangeCellsCount, new int[] { rows, columns }));
			}
		}
		void SetGameStatus(GameStatuses value, object args = null)
		{
			if (GameStatus == value)
				return;
			GameStatus = value;
			if (args == null)
				ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.GameStatus, value));
			else
				ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.GameStatus, new object[] { value, args }));
		}

		void SetCurrentGamerIndex(int value)
		{
			value %= Gamers.Length;
			if (CurrentGamerIndex == value)
				return;

			CurrentGamerIndex = value;

			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.CurrentGamerIndex, CurrentGamerIndex));
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.CurrentGamerId, CurrentGamer.Id));
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.CurrentGamer, CurrentGamer));
		}

		//нет ссылок
		void SetChangeGamerIsTurn(int index, bool isTurn)
		{
			if (Gamers[index].IsTurn == isTurn)
				return;

			var gamerOld = Gamers[index];
			var gamerNew = new UserDto(gamerOld.Id, gamerOld.UserName, gamerOld.ImageIndex, gamerOld.Turn, isTurn, gamerOld.CellType);
			Gamers[index] = gamerNew;
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.ChangeGamerIsTurn, gamerOld, gamerNew));
		}

		void SetTypes(ISet<CellTypeDto> value)
		{
			if (Types == value)
				return;
			Types = value;
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.Types, value));
		}
		protected CellDto[,] Cells;

		protected int CurrentUserID;

		protected IReposSaveGame repos;
		protected SavedGameDto savedGame;
		/// <summary>Конструктор модели</summary>
		/// <param name="rowsCount">Колиичество строк</param>
		/// <param name="columnsCount">Количество колонок</param>
		/// <param name="lineLength">Минимальная длина линии из однородных элементов, необходимая для учета ее завершенности</param>
		public ModelTicTacToe(IReposSaveGame repos)
		{
			this.repos = repos;
		}

		public void CreateGame()
		{
			ChangeCellsCount(RowsCount, ColumnsCount);
			for (int row = 0; row < RowsCount; row++)
			{
				for (int column = 0; column < ColumnsCount; column++)
					SetCellType(new CellDto(row * ColumnsCount + column, column, row, null));
			}
		}

		public void StartNewGame(params object[] args)
		{
			RowsCount = (int)args[0];
			ColumnsCount = (int)args[1];
			LineLength = (int)args[2];
			Gamers = ((IEnumerable<UserDto>)args[3]).OrderBy(gmr => gmr.Turn).ToArray();
			int gamerInd = Gamers.TakeWhile(gmr => !gmr.IsTurn).Count();
			SetCurrentGamerIndex(gamerInd);
			CreateGame();
			SetGameStatus(GameStatuses.Game);
		}

		public bool CanMove(CellDto cell, UserDto user)
		{
			if (user==null || CurrentGamer == null || user.UserName != CurrentGamer.UserName || GameStatus != GameStatuses.Game)
				return false;

			return Cells[cell.Row, cell.Column].CellType == null;
		}

		public void Move(CellDto cell, UserDto user)
		{
			if (!CanMove(cell, user))
				throw new Exception("Данный ход не возможен. Игрок: " + user.Id + ", column: " + cell.Column + ", row: " + cell.Row);
			SetCellType(cell, CurrentGamer.CellType);
			FinishGame(Cells[cell.Row, cell.Column]);
			if (GameStatus == GameStatuses.Game)
				SetCurrentGamerIndex(CurrentGamerIndex +1);
		}
		private void FinishGame(CellDto testCell)
		{
			bool isWin = WinCheck(testCell);
			if (isWin)
			{
				SetGameStatus(GameStatuses.Win, CurrentGamerId);
				return;
			}
			if (!Cells.Cast<CellDto>().Any(cl => cl.CellType == null))
			{
				SetGameStatus(GameStatuses.Draw);
			}
		}

		private bool WinCheck(CellDto cell)
		{

			if (cell.CellType == null) throw new Exception("Попытка проверки пустой ячейки");
			bool horizontal = TestLine(cell, LineLength, true, false, false);
			bool vertical = TestLine(cell, LineLength, false, true, false);
			bool diagonalRight= TestLine(cell, LineLength, true, true, true);
			bool diagonalLeft = TestLine(cell, LineLength, true, true, false);
			return horizontal || vertical || diagonalLeft || diagonalRight;
			/*return (
				TestLine(cell, LineLength, true, false, false) ||   //horizontal
				TestLine(cell, LineLength, false, true, false) ||   //vertical
				TestLine(cell, LineLength, true, true, false) ||    //diagonal-1
				TestLine(cell, LineLength, true, true, true)        //diagonal-2
				);*/
		}

		/// <summary>Проверка на появление новой завершенной линии относительно свежедобавленного элемента</summary>
		/// <param name="cell">ячейка, относительно которой осуществляется проверка</param>
		/// <param name="elementsCount">количество подряд идущих однотипных элементов в ряду, необходимых для зачисления новой линии</param>
		/// <param name="useShiftX">использование при проверке сдвига по оси X</param>
		/// <param name="useShiftY">использование при проверке сдвига по оси Y</param>
		/// <param name="directionForDiagonalTest">должно быть true при проверке диагонали, идущей с верхнего правого конца относительно выбранной ячейки к левому нижнему концу</param>
		/// <returns>Возвращает true в случае появления хоть одной новой заполненной линии</returns>
		private bool TestLine(CellDto cell, int elementsCount, bool useShiftX, bool useShiftY, bool directionForDiagonalTest)
		{
			int ShiftForCalculateCompleteLine = LineLength - 1;//сдвиг относительно проверяемой ячейки


			//Utils.Log("start test line************************************", useShiftX, useShiftY, directionForDiagonalTest);
			int shiftFromX = 0;//точка начала проверки по оси X
			int shiftFromY = (useShiftY == true) ? cell.Row - ShiftForCalculateCompleteLine : 0;//точка начала проверки по оси Y - вычисляется сразу т.к. параметр directionForDiagonalTest не влияет на расчеты по оси Y
			int diagonalFactor = 1;//коэффициент для расчета в случае проверки совпадений по 2й диагонали
			int countCoinCidencesInLine = 0;//количество совпадений в линии

			int countLinesComplete = 0;
			if (useShiftX == true)//смещение по оси X
			{
				if (directionForDiagonalTest == true)//расчет по диагонали с правой стороны
				{
					shiftFromX = cell.Column + ShiftForCalculateCompleteLine;
					diagonalFactor = -1;
				}
				else
				{
					shiftFromX = cell.Column - ShiftForCalculateCompleteLine;
				}
			}
			int length = ShiftForCalculateCompleteLine * 2 + 1;//количество определяемых ячеек - длина возможной линии в обе стороны (с учетом текущей ячейки)

			for (int i = 0; i < length; i++)
			{
				int x = (useShiftX) ? shiftFromX + (diagonalFactor * i) : cell.Column;//координаты проверяемой ячейки по оси X
				int y = (useShiftY) ? shiftFromY + i : cell.Row;//координаты проверяемой ячейки по оси Y
				CellDto targetCell = GetCellByPosiotion(x, y);
				//Utils.Log("test cell (x, y):", x, y);
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

		private CellDto GetCellByPosiotion(int row, int column)
		{
			if (row < 0 || row >= RowsCount || column < 0 || column >= ColumnsCount)
			{
				return null;
			}
			return Cells[row, column];
		}

		public void GamerSurrender()
		{
			SetCurrentGamerIndex(CurrentGamerIndex + 1);
			SetGameStatus(GameStatuses.Win, CurrentGamerId);
		}

		protected bool IsRevenge;
		public void Save()
		{
			repos.Save(new SavedGameDto
			(
				Gamers.ToHashSet(),
				Cells.Cast<CellDto>().Where(cl => cl.CellType != null).ToHashSet(),
				Types,
				RowsCount,
				ColumnsCount,
				LineLength
			));

		}
		private void RemoveSavedFile()
		{
			repos.RemoveSavedGame();
			SetIsRevenge(false);
		}

		public void RepairGame()
		{
			if (!IsRevenge || savedGame == null)
				return;

			
			SetRowsCount(savedGame.RowsCount);
			SetColumnsCount(savedGame.ColumnsCount);
			SetLineLength(savedGame.LengthLineForWin);
			
			SetTypes(savedGame.Types);
			SetGamers(savedGame.Users);
			ChangeCellsCount(RowsCount, ColumnsCount);
			foreach (CellDto cell in savedGame.Cells)
			{
				SetCellType(cell);
			}
			int gamerInd = Gamers.TakeWhile(gmr => !gmr.IsTurn).Count();
			SetCurrentGamerIndex(gamerInd);
			SetGameStatus(GameStatuses.Game);
		}

		private void SetGamers(ISet<UserDto> value)
		{
			Gamers = value.OrderBy(i=>i.Turn).ToArray();
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.Gamers, value));
		}

		private void SetLineLength(int value)
		{
			if (LineLength == value) return;
			LineLength = value;
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.LineLength, value));
		}

		private void SetColumnsCount(int value)
		{
			if (ColumnsCount == value) return;
			ColumnsCount = value;
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.ColumnsCount, value));
		}

		private void SetRowsCount(int value)
		{
			if (RowsCount == value) return;
			RowsCount = value;
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.RowsCount, value));
		}

		public void Load()
		{
			savedGame = repos.Load();
			SetIsRevenge(savedGame != null);
		}
	}


}
