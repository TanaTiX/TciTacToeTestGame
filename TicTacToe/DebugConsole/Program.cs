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
						StartNewGame(UserType.User0);
						break;
					case "new 0":
						StartNewGame(UserType.UserX);
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

		private static void Move(string command)
		{
			try
			{
				/*CellContent content = CellContent.Empty;
				char type = command[0];

				if (type == 'x')
				{
					content = CellContent.Cross;
				}
				else if (type == '0')
				{
					content = CellContent.Zero;
				}
				else
				{
					throw new Exception("Введено недопустимое значение поля клетки");
				}
*/
				int x = Int32.Parse(command[0].ToString());
				int y = Int32.Parse(command[1].ToString());

				CellContent content = (_currentUser == UserType.UserX) ? CellContent.Cross : CellContent.Zero;
				CellDto cell = new CellDto(x, y, content);
				bool moveComplete = model.Move(cell, _currentUser);
				if(moveComplete == false)
				{
					Utils.Log("Ход не совершен");
					return;
				}
				var state = model.Cells;
				for (int i = 0; i < state.Count; i++)
				{
					string res = "";
					string symbol;
					ReadOnlyCollection<CellDto> row = state[i];
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
				_currentUser = (_currentUser == UserType.User0) ? UserType.UserX : UserType.User0;
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.Message);
			}
		}

		private static void ShowInfo()
		{
			Console.WriteLine("Введите new и через пробел икс или ноль для начала новой игры. Икс или ноль определяет то, кто будет ходить первым");
			Console.WriteLine("Введите координаты клетки без пробелов и нажмите энтер");
			Console.WriteLine("clear - очистка консоли от текста");
			Console.WriteLine("new - создание новой игры");
		}
		private static void StartNewGame(UserType user)
		{
			_currentUser = (user == UserType.UserX) ? UserType.User0 : UserType.UserX;
			model = new Model(3, 3, user, 3);
			//model.GameOverWinEvent += OnGameOverWin;
			//model.GameOverDrawEvent += OnGameOverDraw;
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
				case GameStatuses.Win:
					Utils.Log("Победа!");
					break;
				case GameStatuses.Draw:
					Utils.Log("Ничья");
					break;
				case GameStatuses.Cancel:
					Utils.Log("Игра отменана");
					break;
				default:
					break;
			}
		}

		private static void OnGameOverDraw(object sender)
		{
			ClearListeners();
			Console.WriteLine("Игра завершена вничью");
			//Clear();
		}

		private static void OnGameOverWin(object sender)
		{

			
			Console.WriteLine("Game over");
			//Clear();
		}

		private static void ClearListeners()
		{
			//model.GameOverWinEvent -= OnGameOverWin;
			//model.GameOverDrawEvent -= OnGameOverDraw;
			model.ChangeStatusEvent -= OnChangeGameStatus;
		}
	}
}
