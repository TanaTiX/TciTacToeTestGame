using ModelLibrary;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LibVM
{
	public interface IStatisticVM
	{
		/// <summary>Список игроков</summary>
		ObservableCollection<UserStatistic> Users { get; }

		/// <summary>Вернуться на 1ю панель</summary>
		ICommand ShowFirstScreenCommand { get; }

		void LoadStatistic();

	}
}
