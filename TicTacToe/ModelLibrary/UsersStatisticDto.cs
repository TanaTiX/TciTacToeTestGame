using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
	public class UsersStatisticDto
	{
		internal string Name;
		private List<UserStatisticDto> _usersStatistic = new List<UserStatisticDto>();

		public List<UserStatisticDto> UsersStatistic { get => _usersStatistic; set => _usersStatistic = value; }
	}
}
