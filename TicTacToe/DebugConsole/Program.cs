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
			Console.WriteLine("test");
			var line = Console.ReadLine();
			if (line == "clear")
			{
				Clear();
			}
			else
			{
				string[] arr = line.Split(',');
				foreach (string item in arr)
				{
					item.Trim();
				}
				int x = Int32.Parse(arr[1]);
				int y = Int32.Parse(arr[2]);
				CellContent content = CellContent.Empty;
				string type = arr[0];
				if (type == "x")
				{
					content = CellContent.Cross;
				}
				if(type == "y")
				{
					content = CellContent.Zero;
				}
				CellDto cell = new CellDto(x, y, content);
				model.Move(cell);
				
			}
		}
		static void Clear()
		{
			model = new Model(3, 3, 3);
		}
	}
}
