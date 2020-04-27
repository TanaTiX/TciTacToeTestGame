using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace View
{
	public class GameEndVM : IGameEndVM
	{
		public ICommand StartNewGameCommand { get; }

		public ICommand ShowFirstScreenCommand { get; }

		public Gamer Winner => new Gamer()
		{
			UserName = "ИмяПобедителя"
		};

		public Gamer Loser => new Gamer()
		{
			UserName = "ИмяПроигравшего"
		};
	}
}
