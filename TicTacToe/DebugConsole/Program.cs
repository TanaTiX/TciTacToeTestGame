using Common;
using CommonUtils;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugConsole
{
	class Program
	{
		private static IModel model;
		private static UserType _currentUser;
		private const int rowsCount = 3;
		private const int colsCount = 3;
		private static CellView[][] cells;
		static void Main(string[] args)
		{
			ShowInfo();
			
			while (true)
			{
				var command = Console.ReadLine().ToLower().Trim();
				switch (command)
				{
					case "clear":
						Console.Clear();
						break;
					case "new x":
						StartNewGame(UserType.UserSecond);
						break;
					case "new 0":
						StartNewGame(UserType.UserFirst);
						break;
					case "cancel":
						Cancel();
						break;
					case "help":
						ShowInfo();
						break;
					default:
						{
							if(model != null)
							{
								Move(command);
							}
							else
							{
								Utils.Log("Снначала необходимо создать игру");
							}
							break;
						}
				}
			}
			
		}
		private static void ShowInfo()
		{
			Console.WriteLine("ПОМОЩЬ");
			Console.WriteLine("Введите new и через пробел икс или ноль для начала новой игры. Икс или ноль определяет то, кто будет ходить первым");
			Console.WriteLine("Что бы сделать ход введите координаты клетки без пробелов и нажмите энтер. Возможно только при наличии не оконченной игры");
			Console.WriteLine("clear - очистка консоли от текста");
			Console.WriteLine("new - создание новой игры");
			Console.WriteLine("cancel - для отмены игры. Отменить игру можно только если она не началась. Иначе будет засчитано поражение.");
			Console.WriteLine("help - для вывода справки");
		}
		private static void Cancel()
		{
			if (model != null)
			{
				if(model.GameStatus == GameStatuses.New || model.GameStatus == GameStatuses.Zero)
				{
					model = null;
					Utils.Log("Игра отменена");
				}
				else
				{
					Utils.Log("В случае отмены вам будет засчитано поражение. Если вы все равно хотите прекратить игру, то нажмите Y");
					string answer = Console.ReadLine().ToLower().Trim();
					if(answer == "y")
					{
						Console.WriteLine("Вы сдались, засчитано поражение");
						model.CancelGame();
					}

				}
			}
			else
			{
				Utils.Log("Игра не создана, отменять нечего");
			}
		}
		private static void Move(string command)
		{
			try
			{
				int x = Int32.Parse(command[0].ToString());
				int y = Int32.Parse(command[1].ToString());

				CellContent content = (_currentUser == UserType.UserFirst) ? CellContent.Cross : CellContent.Zero;
				CellDto cell = new CellDto(x, y, content);
				
				CellView cellView = new CellView(cell);
				cells[y][x] = cellView;
				
				for (int i = 0; i < cells.Length; i++)
				{
					string res = "";
					string symbol;
					CellView[] row = cells[i];
					for (int j = 0; j < row.Count(); j++)
					{
						CellContent cellContent = row[j].CellType;
						symbol = "-";
						if (cellContent == CellContent.Cross)
						{
							symbol = "X";
						}
						if (cellContent == CellContent.Zero)
						{
							symbol = "0";
						}

						res += symbol;

					}
					Console.WriteLine(res);
				}
				Console.WriteLine("************************** - ход совершен");
				
				_currentUser = (_currentUser == UserType.UserSecond) ? UserType.UserFirst : UserType.UserSecond;
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.Message);
			}
		}

		
		private static void StartNewGame(UserType user)
		{
			if(model!=null && model.GameStatus == GameStatuses.Game)
			{
				Utils.Log("Введена не корректная последовательность команд, игра не начата");
				return;
			}
			
			_currentUser = (user == UserType.UserFirst) ? UserType.UserSecond : UserType.UserFirst;

			cells = new CellView[rowsCount][];

			for (int row = 0; row < rowsCount; row++)
			{
				cells[row] = new CellView[colsCount];
				for (int column = 0; column < colsCount; column++)
					cells[row][column] = new CellView(row, column, CellContent.Empty);
			}

			model = new Model(3, 3, user, 3);
			model.ChangeStatusEvent += OnChangeGameStatus;
		}

		private static void OnChangeGameStatus(object sender, GameStatuses status)
		{
			Utils.Log("change status", status);
			switch (status)
			{
				case GameStatuses.Zero:
					Utils.Log("Такого быть не должно");
					break;
				case GameStatuses.New:
					Utils.Log("Новая игра создана");
					break;
				case GameStatuses.Game:
					Utils.Log("Можно ходить");
					break;
				case GameStatuses.WinFirst:
					Utils.Log("Победа!", _currentUser);
					break;
				case GameStatuses.WinSecond:
					Utils.Log("Победа!", _currentUser);
					break;
				case GameStatuses.Draw:
					Utils.Log("Ничья");
					break;
				case GameStatuses.Cancel:
					Utils.Log("Игра отменена");
					break;
				default:
					break;
			}
		}

		private static void ClearListeners()
		{
			model.ChangeStatusEvent -= OnChangeGameStatus;
		}
	}
}
