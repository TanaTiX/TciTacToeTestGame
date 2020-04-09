using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace View
{
	public class GameEndVM : IGameEndVM
	{
		public ICommand ShowFirstScreenCommand { get; }

		public ICommand RevengeCommand { get; }

		public User Winner => new User { Name = "Вася", Lose = 5, Total = 22, Win = 15 };

		public User Loser => new User { Name = "Пупкин", Lose = 11, Total = 13, Win = 1 };
	}
}
