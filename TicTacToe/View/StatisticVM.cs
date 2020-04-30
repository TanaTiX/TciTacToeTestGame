using Common;
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
		public ObservableCollection<User> Users => new ObservableCollection<User> {//удалить?
			new User(){ Name = "Иван", Total=111,  Win=56, Lose=5 },
			new User(){ Name = "Петр", Total=31,  Win=26, Lose=5 },
			new User(){ Name = "Сидор", Total=81,  Win=44, Lose=5 },
			new User(){ Name = "Феофан", Total=1000,  Win=777, Lose=5 },
			new User(){ Name = "Акакий", Total=9999,  Win=56, Lose=888 },
			new User(){ Name = "Ivengo", Total=564,  Win=564, Lose=0 }
		};

		public ICommand ShowFirstScreenCommand { get; }
	}
}
