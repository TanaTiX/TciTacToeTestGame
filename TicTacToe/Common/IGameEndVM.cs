using System.Windows.Input;

namespace Common
{
	public interface IGameEndVM
	{
		/// <summary>Показать исходное окно прогарммы</summary>
		ICommand ShowFirstScreenCommand { get; }

		/// <summary>Команда для начала новой игры - реванш - с прежними настройками (игроки)</summary>
		ICommand RevengeCommand { get; }
	}
}
