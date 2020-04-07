using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Common
{
	public interface IFirstScreen
	{
		/// <summary>Список игроков</summary>
		ObservableCollection<User> users { get; }

	}
}
