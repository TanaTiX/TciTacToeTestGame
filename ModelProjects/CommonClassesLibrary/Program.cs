using ClassesLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClassesLibrary
{
	class Program
	{
		public delegate void GameOver(object sender, bool isWin);
		public delegate void Move(object sender, CellDto cell);
		static void Main(string[] args)
		{
			Console.ReadKey();
		}
	}
}
