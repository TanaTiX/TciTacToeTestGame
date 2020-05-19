using Common;
using LibVM;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace View
{
	public class StatisticVM : IStatisticVM
	{
		public ObservableCollection<UserStatistic> Users => new ObservableCollection<UserStatistic> {//удалить?
			new UserStatistic(){ Name = "Иван", Draw=111,  Win=56, Lose=5 },
			new UserStatistic(){ Name = "Петр", Draw=31,  Win=26, Lose=5 },
			new UserStatistic(){ Name = "Сидор", Draw=81,  Win=44, Lose=5 },
			new UserStatistic(){ Name = "Феофан", Draw=1000,  Win=777, Lose=5 },
			new UserStatistic(){ Name = "Акакий", Draw=9999,  Win=56, Lose=888 },
			new UserStatistic(){ Name = "Ivengo", Draw=564,  Win=564, Lose=0 }
		};

		public ICommand ShowFirstScreenCommand { get; }
	}
}
