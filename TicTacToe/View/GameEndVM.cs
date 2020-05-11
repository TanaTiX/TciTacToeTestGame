using Common;
using LibVM;
using ModelLibrary;
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

		public UserVM Winner => new UserVM()
		{
			UserName = "ИмяПобедителя"
		};

		public UserVM Loser => new UserVM()
		{
			UserName = "ИмяПроигравшего"
		};
	}
}
