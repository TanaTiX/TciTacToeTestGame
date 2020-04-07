using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Common
{
	public interface IFirstScreen
	{
		/// <summary>Список игроков</summary>
		ObservableCollection<User> users { get; }

		/// <summary>Вернуться на 1ю панель</summary>
		ICommand ExitStatisticCommand { get; }

	}
}
