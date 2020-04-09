using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Common
{
	public interface IFirstScreenVM
	{
		/// <summary>Команда начала новой игры - должно открываться новое коно</summary>
		ICommand SrartNewGameCommand { get; }

		/// <summary>Команда восстановления ранее не оконченной игры</summary>
		ICommand RepairGameCommand { get; }

		
		/// <summary>Команда об отображении панели с настройками</summary>
		ICommand ShowSettingsCommand { get; }

		/// <summary>Команда об отображении панели со статистикой пользователей</summary>
		ICommand ShowStatisticCommand { get; }

		///<summary>Свойство, отражающее наличие несохраненной игры</summary>
		bool IsRevenge { get; }

	}
}
