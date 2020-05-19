using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibVM
{
	public class UserStatisticVM
	{
		public ObservableCollection<UserStatisticVM> Users { get; }
			= new ObservableCollection<UserStatisticVM>();
	}
}
