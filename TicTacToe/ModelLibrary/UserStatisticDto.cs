using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
	public class UserStatisticDto
	{
		private string _name;
		private int _win;
		private int _lose;
		private int _draw;
		public string Name { get=>_name; private set { _name = value; } }
		public int Draw { get => _draw; private set { _draw = value; } }
		public int Win { get => _win; private set { _win = value; } }
		public int Lose { get => _lose; private set { _lose = value; } }

		public static UserStatisticDto Create(string name, int win, int lose, int draw)
		{
			UserStatisticDto dto = new UserStatisticDto()
			{
				Name = name,
				Win = win,
				Lose = lose,
				Draw = draw
			};
			return dto;
		}
	}
}
