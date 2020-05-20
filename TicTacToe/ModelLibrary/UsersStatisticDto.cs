using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
	public class UsersStatisticDto
	{
		public UsersStatisticDto(Dictionary<string, UserStatisticDto> statistic)
		{
			_usersStatistic = statistic;
		}
		//internal string Name;
		private Dictionary<string, UserStatisticDto> _usersStatistic;

		public Dictionary<string, UserStatisticDto> UsersStatistic { get => _usersStatistic; }
	}
}
