using Common;
using LibVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace View
{
	public class FirstScreenVM : IFirstScreenVM
	{
		public ICommand StartNewGameCommand { get; }

		public ICommand RepairGameCommand { get; }

		public ICommand ShowSettingsCommand { get; }

		public ICommand ShowStatisticCommand { get; }

		public bool IsRevenge => true;
	}
}
