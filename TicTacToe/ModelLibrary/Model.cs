using Common;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using CommonUtils;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using System.IO;

namespace ModelLibrary
{
	public class Model : IModel
	{
		public int RowsCount { get; }
		public int ColumnsCount { get; }
		public int LineLength { get; }
		public GameStatuses GameStatus { get; private set; }


		private readonly int ShiftForCalculateCompleteLine;//сдвиг относительно проверяемой ячейки

		public Gamer FirstGamer { get; private set; }
		public Gamer SecondGamer { get; private set; }

		private int TotalFreeCells;

		//public event GameOverHandler GameOverWinEvent;
		//public event GameOverDrawHandler GameOverDrawEvent;
		public event MoveHandler MoveEvent;
		public event ChangeStatusHandler ChangeStatusEvent;

		private readonly ReadOnlyCollection<CellDto[]> cellsArray;
		public ReadOnlyCollection<ReadOnlyCollection<CellDto>> Cells { get; }

		//private UserType _currentUser;
		public UserType CurrentUser { get; set; }

		/// <summary>Конструктор модели</summary>
		/// <param name="rowsCount">Колиичество строк</param>
		/// <param name="columnsCount">Количество колонок</param>
		/// <param name="secondUser">Игрок, который считается походившим. Следующий ход должен быть другим игроком.</param>
		/// <param name="lineLength">Минимальная длина линии из однородных элементов, необходимая для учета ее завершенности</param>
		public Model(int rowsCount, int columnsCount, UserType secondUser, int lineLength = 3)
		{
			if (secondUser == UserType.Unknown) throw new Exception("Не допустимое значение 1-го игрока");
					IsRevenge = false;
			try
			{
				if (File.Exists(Model.FileNameXml))
				{
					using (var file = File.OpenRead(FileNameXml))
						_ = (SaveGame)serializer.Deserialize(file);
					IsRevenge = true;
				}

			}
			catch (Exception ex){}

			CurrentUser = secondUser;
			RowsCount = rowsCount;
			ColumnsCount = columnsCount;
			TotalFreeCells = RowsCount * ColumnsCount;
			LineLength = lineLength;
			ShiftForCalculateCompleteLine = LineLength - 1;

			// Делаем временный массив и заполняем его
			var cells = new CellDto[ColumnsCount][];

			for (int column = 0; column < RowsCount; column++)
			{
				cells[column] = new CellDto[RowsCount];
				for (int row = 0; row < rowsCount; row++)
					cells[column][row] = new CellDto(row, column, CellContent.Empty);
			}
			/*for (int row = 0; row < RowsCount; row++)
			{
				cells[row] = new CellDto[ColumnsCount];
				for (int column = 0; column < ColumnsCount; column++)
					cells[row][column] = new CellDto(row, column, CellContent.Empty);
			}*/

			/// Так как строки не меняются, то преобразуем 
			/// временный массив в неизменяемый по первому измерению
			cellsArray = Array.AsReadOnly(cells);

			/// Публичный массив делаем неизменяемым по двум измерениям.
			/// При этом у нас сохраняется связь с элементами внутреннего массива,
			/// так Array.AsReadOnly(_row)) делает не копию, а оболочку надо 
			/// массивом каждой строки. И изменяя значения в исходном массиве мы их меняем 
			/// и в публичном. Но "из вне" эти значения поменять не могут.
			Cells = Array.AsReadOnly(cells.Select(_row => Array.AsReadOnly(_row)).ToArray());
			SetStatus(GameStatuses.New);
		}

		public void CreateGame(Gamer firstGamer, Gamer secondGamer)
		{
			FirstGamer = firstGamer;
			SecondGamer = secondGamer;
			TotalFreeCells = ColumnsCount * RowsCount;
			for (int row = 0; row < RowsCount; row++)
			{
				for (int column = 0; column < ColumnsCount; column++)
					cellsArray[row][column] = new CellDto(column, row, CellContent.Empty);
			}
		}

		public void StartNewGame(/*Gamer firstGamer, Gamer secondGamer*/)
		{
			//FirstGamer = firstGamer;
			//SecondGamer = secondGamer;
			//TotalFreeCells = ColumnsCount * RowsCount;
			/*for (int row = 0; row < RowsCount; row++)
			{
				for (int column = 0; column < ColumnsCount; column++)
					cellsArray[row][column] = new CellDto(column, row, CellContent.Empty);
			}*/

			SetStatus(GameStatuses.Game);
		}

		public bool CanMove(CellDto cell, UserType user)
		{
			if (user != CurrentUser)
			{
				return false;
				//throw new Exception("Ход вне очереди");//По хорошему все эти и аналогичные сообщения нужно вынестти в отдельный список, но пока не буду заморачиваться
			}
			return Cells[cell.Row][cell.Column].CellType == CellContent.Empty;
		}

		public void Move(CellDto cell, UserType user)
		{
			//Utils.Log("x", cell.X, "y", cell.Y);
			if (cell.CellType != CellContent.Empty)
			{
				throw new Exception("Произведена попытка хода пустой клеткой");
			}
			if (!CanMove(cell, user))
				throw new Exception("Данный ход не возможен. Игрок: " + user + ", column: " + cell.Column + ", row: " + cell.Row);

			SetStatus(GameStatuses.Game);
			TotalFreeCells--;
			CellDto newCell;
			if (CurrentUser == UserType.UserFirst)
			{
				newCell = new CellDto(cell.Column, cell.Row, CellContent.Cross);
			}
			else if (CurrentUser == UserType.UserSecond)
			{
				newCell = new CellDto(cell.Column, cell.Row, CellContent.Zero);
			}
			else
			{
				throw new Exception("Нет польхователя для хода");
			}
			cellsArray[cell.Row][cell.Column] = newCell;
			//CurrentUser = user;
			MoveEvent?.Invoke(this, newCell);
			FinishGame(newCell);
		}
		private void FinishGame(CellDto testCell)
		{

			bool isWin = WinCheck(testCell);
			if (isWin == false && TotalFreeCells == 0)
			{
				SetStatus(GameStatuses.Draw);
			}
			if (isWin)
			{
				SetStatus(CurrentUser == UserType.UserFirst ? GameStatuses.WinFirst : GameStatuses.WinSecond);
			}
			CurrentUser = CurrentUser == UserType.UserFirst ? UserType.UserSecond : UserType.UserFirst;
			RemoveSavedFile();
		}

		private bool WinCheck(CellDto cell)
		{
			if (cell.CellType == CellContent.Empty) throw new Exception("Попытка проверки пустой ячейки");
			bool isWin = (
				TestLine(cell, LineLength, false, true, false) ||   //vertical
				TestLine(cell, LineLength, true, false, false) ||   //horizontal
				TestLine(cell, LineLength, true, true, false) ||    //diagonal-1
				TestLine(cell, LineLength, true, true, true)        //diagonal-2
				);

			if (isWin == true)
			{
				//var sel = from p in cells where p
				//List<CellDto> bufer = Cells.Cast<CellDto>().ToList();
				var bufer = Cells.Concat();
				int countMoves = bufer.Where(p => p.CellType != CellContent.Empty).Count();
				if (countMoves % 2 == 1)//TODO: проверить правильность
				{
					SetStatus(GameStatuses.WinFirst);
				}
				else
				{
					SetStatus(GameStatuses.WinSecond);
				}
				//GameOverWinEvent?.Invoke(this);

				return true;
			}
			return false;
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

		private CellDto GetCellByPosiotion(int x, int y)
		{
			if (x < 0 || x >= ColumnsCount || y < 0 || y >= RowsCount)
			{
				return null;
			}
			return Cells[y][x];
		}

		private void SetStatus(GameStatuses status)
		{
			if (GameStatus == status) return;
			/*switch (status)
			{
				case GameStatuses.Zero:
					throw new Exception("Невозможная последовательность смены состояния игры");
				case GameStatuses.New:
					if (GameStatus == GameStatuses.Game)
						throw new Exception("Невозможная последовательность смены состояния игры");
					break;
				//case GameStatuses.Game:
					//if (GameStatus != GameStatuses.New)
						//throw new Exception("Невозможная последовательность смены состояния игры");
					//break;
				case GameStatuses.WinFirst:
					if (GameStatus != GameStatuses.Game)
						throw new Exception("Невозможная последовательность смены состояния игры");
					break;
				case GameStatuses.WinSecond:
					if (GameStatus != GameStatuses.Game)
						throw new Exception("Невозможная последовательность смены состояния игры");
					break;

				case GameStatuses.Draw:
					if (GameStatus != GameStatuses.Game)
						throw new Exception("Невозможная последовательность смены состояния игры");
					break;
				case GameStatuses.Cancel:
					if (GameStatus != GameStatuses.New)
						throw new Exception("Невозможная последовательность смены состояния игры");
					break;
				default:
					break;
			}*/
			GameStatus = status;
			ChangeStatusEvent?.Invoke(this, status);
			Utils.Log("status changed in model:", status);
		}

		public void CancelGame()
		{
			//List<CellDto> bufer = Cells.Cast<CellDto>().ToList();
			var bufer = Cells.Concat();
			int countMoves = bufer.Where(p => p.CellType != CellContent.Empty).Count();
			if (countMoves % 2 == 1)//TODO: проверить правильность
			{
				SetStatus(GameStatuses.WinFirst);
			}
			else
			{
				SetStatus(GameStatuses.WinSecond);
			}
			//SetStatus(GameStatuses.Win);
		}


		public static string FileNameXml { get; set; }

		//private bool _isRevenge = false;

		public bool IsRevenge { get; private set; }

		protected static readonly XmlSerializer serializer = new XmlSerializer(typeof(SaveGame));
		public void Save()//Сохранять игру должно в автоматическом режиме, по хорошему это должен делать, как и загрузку результатов, отдельный объект. Поэтому не понятно, необхоимо ли это свойство в интерфейсе. Ведь достаточно приватного метода.
		{
			if(GameStatus != GameStatuses.Game)
			{
				return;
			}
			if(Cells.Concat().Where(p=> p.CellType == CellContent.Empty).Count() == RowsCount * ColumnsCount)
			{
				return;
			}


			SaveGame saveGame = new SaveGame();
			saveGame.FirstUser = FirstGamer.UserName;
			saveGame.SecondUser = SecondGamer.UserName;
			saveGame.ImageIndexFirstUser = FirstGamer.ImageIndex;
			saveGame.ImageIndexSecondUser = SecondGamer.ImageIndex;
			saveGame.IsCurrentFirstUtser = CurrentUser == UserType.UserFirst;
			saveGame.Cells = CellXML.CreateCells(Cells);

			using (var file = File.Create(FileNameXml))
				serializer.Serialize(file, saveGame);

		}
		public SaveGame Load(Gamer firstGamer, Gamer secondGamer)
		{
			SaveGame saveGame;

			try
			{
				using (var file = File.OpenRead(FileNameXml))
					saveGame = (SaveGame)serializer.Deserialize(file);

				for (int i = 0; i < saveGame.Cells.Count(); i++)
				{
					string xmlType = saveGame.Cells[i].CellType;
					CellContent type = (CellContent)Enum.Parse(typeof(CellContent), xmlType); 
					//switch (xmlType)
					//{
					//	case "Cross": type = CellContent.Cross; break;
					//	case "Zero": type = CellContent.Zero; break;
					//	case "Empty": type = CellContent.Empty; break;
					//	default: throw new Exception("При попытке загрузки сохраненной игры возникла ошибка");
					//}
					//model.Cells = new CellDto(Cells[i].Column, Cells[i].Row, type);
					cellsArray[i / 3][i % 3] = new CellDto(i%3, i/3, type);
				}
				FirstGamer = firstGamer;
				SecondGamer = secondGamer;
				FirstGamer.UserName = saveGame.FirstUser;
				SecondGamer.UserName = saveGame.SecondUser;
				FirstGamer.ImageIndex = saveGame.ImageIndexFirstUser;
				SecondGamer.ImageIndex = saveGame.ImageIndexSecondUser;
				CurrentUser = saveGame.IsCurrentFirstUtser ? UserType.UserFirst : UserType.UserSecond;
				SetStatus(GameStatuses.Game);
				return saveGame;
			}
			catch (Exception)
			{
				return null;
			}
		}

		private void RemoveSavedFile()
		{
			if (File.Exists(FileNameXml))
			{
				try
				{
					File.Delete(FileNameXml);
					IsRevenge = false;
				}
				catch (Exception ex)
				{

					throw ex;
				}
			}
		}
	}


}
