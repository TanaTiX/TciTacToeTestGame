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
				if (line == "clear")
				{
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
						}else if (type == 'y')
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
		}
	}
}
