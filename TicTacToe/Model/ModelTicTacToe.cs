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





	public class ModelTicTacToe : IModel
	{
		/// <summary>Количество строк поля</summary>
		protected int RowsCount;
		/// <summary>Количество колонок поля</summary>
		protected int ColumnsCount;
		/// <summary>Количество идущих подряд ячеек с одним контентом,
		/// достаточное для выигрыша.</summary>
		protected int LineLength;
		/// <summary>Состояние игры.</summary>
		protected GameStatuses GameStatus;
		/// <summary>Зарегистрированные игроки на текущую игру.</summary>
		protected UserDto[] Gamers;
		/// <summary>Допустимы типы для контента ячейки.</summary>
		protected ISet<CellTypeDto> Types;
		/// <summary>Id текущего игрока - того кто должен сделать ход.</summary>
		protected int CurrentGamerId => CurrentGamer.Id;
		/// <summary>Индекс текущего игрока - того кто должен сделать ход.</summary>
		protected int CurrentGamerIndex = -1;
		/// <summary>Текущий игрок - тот чья очередь делать ход.</summary>
		protected UserDto CurrentGamer => Gamers[CurrentGamerIndex];


		//private int ShiftForCalculateCompleteLine;//сдвиг относительно проверяемой ячейки
		//public event NotifyChangedCellHandler ChangedCellEvent;

		/// <summary>Изменение типа содержания заданной ячейки</summary>
		/// <param name="cell">Заданная ячейка</param>
		/// <param name="type">Новый тип содержания</param>
		void SetCellType(CellDto cell, CellTypeDto type)
		{
			// Если текущий тип равен присваиваемому, то ничего не делается.
			// Можно написать сокращённо:
			//if (Cells[cell.Row, cell.Column]?.CellType == type)
			if (Cells[cell.Row, cell.Column] != null && Cells[cell.Row, cell.Column].CellType == type)
				return;

			// В противном случае создаётся новый экземпляр ячейки с новым значением контента.
			// Если ячейка заполнена, то Id берётся из неё. Если не заполнена, то из переданного
			// параметра cell. После присвоения ячейке нового экземпляра, создаётся событие 
			// с передачей нового содержания ячейки.
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.CellType,
				Cells[cell.Row, cell.Column] = new CellDto(Cells[cell.Row, cell.Column]?.Id ?? cell.Id, cell.Row, cell.Column, type)));
		}

		/// <summary>Изменение типа содержания заданной ячейки с передачей нового
		/// типа в свойстве параметра cell</summary>
		/// <param name="cell">Заданная ячейка</param>
		void SetCellType(CellDto cell)
			=> SetCellType(cell, cell.CellType);

		/// <summary>Событие происходящее при любых изменениях в сотонии Модели.</summary>
		public event NotifyChangedStateHandler ChangedStateEvent;

		/// <summary>Изменение состояния флага наличия сохранённой игры.
		/// Название не удачно - желательно поменять.</summary>
		/// <param name="value"><see langword="true"/> - есть сохранённая игра.</param>
		void SetIsGameSaved(bool value)
		{
			// Проверка отличия нового значения от текущего
			if (IsGameSaved == value)
				return;

			// Присваивание нового значение и создание события с новым значением.
			IsGameSaved = value;
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.IsRevenge, value));
		}

		/// <summary>Пересоздание массива ячеек поля игры.</summary>
		/// <param name="rows">Количество строк поля</param>
		/// <param name="columns">Количество колонок поля</param>
		/// <remarks>Пересоздание происходит, если одно из новых значений не совпадает с текущим.</remarks>
		void ChangeCellsCount(int rows, int columns)
		{
			if (Cells == null || Cells.GetLength(0) != rows || Cells.GetLength(1) != columns)
			{
				Cells = new CellDto[rows, columns];
				ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.ChangeCellsCount, new int[] { rows, columns }));
			}
		}

		/// <summary>Изменение состояния игры.</summary>
		/// <param name="value">Новое состояние игры.</param>
		/// <param name="args">Дополнительные аргументы, если надо.</param>
		void SetGameStatus(GameStatuses value, object args = null)
		{
			// Сравнение с текущим состоянием.
			if (GameStatus == value)
				return;

			// Присваивание нового значения состояния.
			GameStatus = value;

			// Создание события с разными параметрами в зависимости от переданного значения args.
			// Если args пустое, то в событие передаётся только новое состояние игры.
			// Иначе - передаётся массив из двух элементов: новое состояние игры и параметр args.
			if (args == null)
			{
				ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.GameStatus, value));
			}
			else
			{
				// Если новое состояние это победа или ничья, то обновляется статистика.
				if (GameStatus == GameStatuses.Win || GameStatus == GameStatuses.Draw)
				{
					if (GameStatus == GameStatuses.Win)
						RepoStatistic.SaveStatistic(Gamers, CurrentGamerId == Gamers[0].Id, CurrentGamerId == Gamers[1].Id);// SaveStatistic();
					else
						RepoStatistic.SaveStatistic(Gamers, false, false);

				}
				ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.GameStatus, new object[] { value, args }));
			}
		}

		/// <summary>Изменение индекса текущего игрока.</summary>
		/// <param name="value">Новый индекс.</param>
		void SetCurrentGamerIndex(int value)
		{
			// Индекс нормализуется по длинне массива Игроков текущей игры.
			value %= Gamers.Length;

			// Проверка индекса на равенство текущему индексу.
			if (CurrentGamerIndex == value)
				return;

			// Если текущий индекс больше нуля, то сбрасывается флаг очереди хода 
			// у игрока с эти индексом.
			if (CurrentGamerIndex >= 0)
				// Здесь была ошибка: нужен сброс флаг.
				//SetChangeGamerIsTurn(CurrentGamerIndex, true);
				SetChangeGamerIsTurn(CurrentGamerIndex, false);

			// Изменение интекса текущего игрока.
			CurrentGamerIndex = value;

			// Если установленный индекс больше нуля, то устанавливается флаг очереди хода 
			// у игрока с эти индексом.
			if (CurrentGamerIndex >= 0)
				SetChangeGamerIsTurn(CurrentGamerIndex, true);

			// Создание событий для для извежения об измении: CurrentGamerIndex, CurrentGamerId и CurrentGamer
			// В VM не совсем правильная обработка этих событий.
			// Из старой версии там частично остался функционал Модели.
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.CurrentGamerIndex, CurrentGamerIndex));
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.CurrentGamerId, CurrentGamer.Id));
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.CurrentGamer, CurrentGamer));

		}

		/// <summary>Изменение флага очереди игры у заданного игрока.
		/// Метод вызывается только из метода SetCurrentGamerIndex.</summary>
		/// <param name="index">Индекс игрока у которого надо изменить флаг.</param>
		/// <param name="isTurn">Новое значение флага.</param>
		void SetChangeGamerIsTurn(int index, bool isTurn)
		{
			// Если флаг имеет такое же значение, то выход.
			if (Gamers[index].IsTurn == isTurn)
				return;

			// Посылка старого и нового значений сделана для примера реализции подобного.
			// В данном случае по существу это не нужно.
			// В VM обрабатывается только новое значение.
			//
			// Запоминается старое значение -  целиком состояние игрока, так как это неизменяемый тип.
			// Создаётся новое значени целиком игрока с новым состоянием флага.
			// Запоминается новое значение под тем же индексом, тем самы достигается изменение только флага.
			// Остальные значения остаются прежними.
			// 
			// Создаётся событие с отправкой старого и нового значений.
			var gamerOld = Gamers[index];
			var gamerNew = new UserDto(gamerOld.Id, gamerOld.UserName, gamerOld.ImageIndex, gamerOld.Turn, isTurn, gamerOld.CellType);
			Gamers[index] = gamerNew;
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.ChangeGamerIsTurn, gamerOld, gamerNew));
		}

		/// <summary>Установка допустимых типов для новой игры.</summary>
		/// <param name="value">Допустимые типы. Id должны быть уникальными.</param>
		void SetTypes(ISet<CellTypeDto> value)
		{
			// Если коллекция таже самая, то выход.
			if (Types == value)
				return;
			Types = value;
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.Types, value));
		}

		/// <summary>Массив ячеек поля игры.</summary>
		protected CellDto[,] Cells;

		/// <summary>Id текущего игрока.</summary>
		protected int CurrentUserID;

		/// <summary>Ссылка на Репозитрий данных игры.</summary>
		protected IReposSaveGame ReposGame;

		/// <summary>Ссылка на Репозитрий статистики игр.</summary>
		protected IReposStatistic RepoStatistic;

		/// <summary>Поле для хранения данных загруженной игры.
		/// Если загруженной игры не было, то - <see langword="null"/>.</summary>
		protected SavedGameDto SavedGame;

		/// <summary>Конструктор модели</summary>
		/// <param name="rowsCount">Колиичество строк</param>
		/// <param name="columnsCount">Количество колонок</param>
		/// <param name="lineLength">Минимальная длина линии из однородных элементов, необходимая для учета ее завершенности</param>
		public ModelTicTacToe(IReposSaveGame reposGame, IReposStatistic reposStatistic)
		{
			ReposGame = reposGame;
			RepoStatistic = reposStatistic;
		}

		/// <summary>Инициализация поля игры при создании новой игры.</summary>
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
			Types = (ISet<CellTypeDto>)args[3];
			Gamers = ((ISet<UserDto>)args[4]).OrderBy(gmr => gmr.Turn).ToArray();
			int gamerInd = Gamers.TakeWhile(gmr => !gmr.IsTurn).Count();
			SetCurrentGamerIndex(gamerInd);
			CreateGame();
			SetGameStatus(GameStatuses.Game);

			/// Установка флага начатой игры
			//SetIsGameSaved(true);
		}

		public bool CanMove(CellDto cell, UserDto user)
		{
			if (user == null || CurrentGamer == null || user.UserName != CurrentGamer.UserName || GameStatus != GameStatuses.Game)
				return false;

			return Cells[cell.Row, cell.Column]?.CellType == null;
		}

		public void Move(CellDto cell, UserDto user)
		{
			if (!CanMove(cell, user))
				throw new Exception("Данный ход не возможен. Игрок: " + user.Id + ", column: " + cell.Column + ", row: " + cell.Row);
			SetCellType(cell, CurrentGamer.CellType);
			FinishGame(Cells[cell.Row, cell.Column]);
			if (GameStatus == GameStatuses.Game)
				SetCurrentGamerIndex(CurrentGamerIndex + 1);
			//else
			//	/// Сброс флага начатой игры
			//	SetIsGameSaved(false);
		}

		private void FinishGame(CellDto testCell)
		{
			bool isWin = WinCheck(testCell);
			if (isWin)
			{
				SetGameStatus(GameStatuses.Win, CurrentGamerId);
				return;
			}
			if (!Cells.Cast<CellDto>().Any(cl => cl?.CellType == null))
			{
				SetGameStatus(GameStatuses.Draw);
			}
		}

		private bool WinCheck(CellDto cell)
		{

			if (cell.CellType == null) throw new Exception("Попытка проверки пустой ячейки");
			bool horizontal = TestLine(cell, LineLength, true, false, false);
			bool vertical = TestLine(cell, LineLength, false, true, false);
			bool diagonalRight = TestLine(cell, LineLength, true, true, true);
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
			//SetIsGameSaved(false);
		}

		/// <summary>Поле флага наличия игры для продолжения.<para/>
		/// Здесь у нас какая-то путаница.
		/// Как по названию, так и по логике.<para/>
		/// Надо точно определиться с логикой, когда этот флаг должен быть установлен, а когда сброшен.<para/>
		/// И, исходя из логики, задать ему правильное название.</summary>
		protected bool IsGameSaved;

		public void Save()
		{
			//if (IsGameSaved)
			{
				/// Проверка флага начатой игры
				if (GameStatus == GameStatuses.Game)
					ReposGame.Save(new SavedGameDto
					(
						Gamers.ToHashSet(),
						Cells.Cast<CellDto>().Where(cl => cl?.CellType != CellTypeDto.Empty).ToHashSet(), Types,
						RowsCount,
						ColumnsCount,
						LineLength
					));


			}
		}

		/// <summary>Вызов метода Репозитория, удаляющего сохранённые данные игры.</summary>
		private void RemoveSavedFile()
		{
			ReposGame.RemoveSavedGame();
			SetIsGameSaved(false);
		}

		public void RepairGame()
		{
			if (!IsGameSaved || SavedGame == null)
				return;


			SetRowsCount(SavedGame.RowsCount);
			SetColumnsCount(SavedGame.ColumnsCount);
			SetLineLength(SavedGame.LengthLineForWin);

			SetTypes(SavedGame.Types);
			SetGamers(SavedGame.Users);
			ChangeCellsCount(RowsCount, ColumnsCount);
			foreach (CellDto cell in SavedGame.Cells)
			{
				SetCellType(cell);
			}
			int gamerInd = Gamers.TakeWhile(gmr => !gmr.IsTurn).Count();
			SetCurrentGamerIndex(gamerInd);
			SetGameStatus(GameStatuses.Game);


			/// Удаление сохранённой игры
			RemoveSavedFile();
		}

		/// <summary>Регистрация списка игроков для новой игры.</summary>
		/// <param name="value">Множество игроков. Id должны быть уникальными.</param>
		private void SetGamers(ISet<UserDto> value)
		{
			Gamers = value.OrderBy(i => i.Turn).ToArray();
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.Gamers, value));
		}

		/// <summary>Изменение выигрышной длины. Задаётся при создании новой игры.</summary>
		/// <param name="value">Длина непрерывной последовательности одного типа.</param>
		private void SetLineLength(int value)
		{
			if (LineLength == value) return;
			LineLength = value;
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.LineLength, value));
		}

		/// <summary>Количество колонок поля игры. Задаётся при создании новой игры.</summary>
		/// <param name="value">Количество колонок.</param>
		private void SetColumnsCount(int value)
		{
			if (ColumnsCount == value) return;
			ColumnsCount = value;
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.ColumnsCount, value));
		}

		/// <summary>Количество строк поля игры. Задаётся при создании новой игры.</summary>
		/// <param name="value">Количество строк.</param>
		private void SetRowsCount(int value)
		{
			if (RowsCount == value) return;
			RowsCount = value;
			ChangedStateEvent?.Invoke(this, new ChangedStateHandlerArgs(NamesState.RowsCount, value));
		}

		public void Load()
		{
			SavedGame = ReposGame.Load();

			SetIsGameSaved(SavedGame != null);
		}

		public UsersStatisticDto LoadStatistic()
		{
			return RepoStatistic.LoadStatistic();
		}
	}


}
