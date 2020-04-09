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
	}
}
