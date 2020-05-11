using Common;
using ModelLibrary;
using System.Windows.Input;

namespace LibVM
{
	public interface IGameEndVM
	{
		/// <summary>Показать исходное окно прогарммы</summary>
		ICommand ShowFirstScreenCommand { get; }

		/// <summary>Команда для начала новой игры - реванш - с прежними настройками (игроки)</summary>
		ICommand StartNewGameCommand { get; }

		/// <summary>Победитель</summary>
		UserVM Winner { get; }

		/// <summary>Проигравший</summary>
		UserVM Loser { get; }
	}
}
