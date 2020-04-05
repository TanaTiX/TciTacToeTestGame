using Common;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugConsole
{
	class Program
	{
		private static IModel model;
		static void Main(string[] args)
		{
			Clear();

			Console.WriteLine("Введите икс или ноль и координаты клетки без пробелов и нажмите энтер");

			while (true)
			{
				var line = Console.ReadLine();
				if (line.ToLower() == "clear")
				{
					Console.Clear();
					Clear();
				}
				else
				{
					try
					{
						CellContent content = CellContent.Empty;
						char type = line[0];

						if (type == 'x')
						{
							content = CellContent.Cross;
						}else if (type == '0')
						{
							content = CellContent.Zero;
						}
						else
						{
							throw new Exception("Введено недопустимое значение поля клетки");
						}

						int x = Int32.Parse(line[1].ToString());
						int y = Int32.Parse(line[2].ToString());

						CellDto cell = new CellDto(x, y, content);
						model.Move(cell);
						var state = model.PublicCells;
						for (int i = 0; i < state.Length; i++)
						{
							string res = "";
							string symbol;
							CellDto[] row = state[i];
							for (int j = 0; j < row.Count(); j++)
							{
								CellContent cellContent = row[j].CellType;
								symbol = "-";
								if (cellContent == CellContent.Cross)
								{
									symbol = "X";
								}
								if(cellContent == CellContent.Zero)
								{
									symbol = "0";
								}

								res += symbol;
								
							}
							Console.WriteLine(res);
						}
					}
					catch (Exception ex)
					{

						Console.WriteLine(ex.Message);
					}

					
				}
			}
			
		}
		static void Clear()
		{
			model = new Model(3, 3, 3);
			model.GameOverWinEvent += OnGameOverWin;
			model.GameOverDrawEvent += OnGameOverDraw;
		}

		private static void OnGameOverDraw(object sender)
		{
			model.GameOverWinEvent -= OnGameOverWin;
			model.GameOverDrawEvent -= OnGameOverDraw;
			Console.WriteLine("Игра завершена вничью");
			Clear();
		}

		private static void OnGameOverWin(object sender)
		{
			
			model.GameOverWinEvent -= OnGameOverWin;
			model.GameOverDrawEvent -= OnGameOverDraw;
			Console.WriteLine("Game over");
			Clear();
		}
	}
}
