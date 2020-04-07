using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Common
{
	public class IFirstScreen
	{
		ICommand CommandSrartNewGame;
		ICommand CommandRepairGame;
		ICommand CommandCancelGame;
		ICommand CommandShowSettings;
		ObservableCollection<User> users;//разве это должно быть в этом объекте?

	}
}
