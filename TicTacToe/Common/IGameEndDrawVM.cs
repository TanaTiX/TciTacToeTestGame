using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Common
{
	public interface IGameEndDrawVM
	{
		/// <summary>Показать исходное окно прогарммы</summary>
		ICommand ShowFirstScreenCommand { get; }

		/// <summary>Команда для начала новой игры - реванш - с прежними настройками (игроки)</summary>
		ICommand StartNewGameCommand { get; }

		/// <summary>Победитель</summary>
		Gamer FirstGamer { get; }

		/// <summary>Проигравший</summary>
		Gamer SecondGamer { get; }
	}
}
