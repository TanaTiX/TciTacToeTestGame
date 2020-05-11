using Common;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibVM
{
	public interface IGameEndDrawVM
	{
		/// <summary>Показать исходное окно прогарммы</summary>
		ICommand ShowFirstScreenCommand { get; }

		/// <summary>Команда для начала новой игры - реванш - с прежними настройками (игроки)</summary>
		ICommand StartNewGameCommand { get; }

		/// <summary>Победитель</summary>
		UserVM FirstGamer { get; }

		/// <summary>Проигравший</summary>
		UserVM SecondGamer { get; }
	}
}
