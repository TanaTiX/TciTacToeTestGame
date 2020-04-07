using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Common
{
	public class IFirstScreenVM
	{
		ICommand SrartNewGameCommand { get; }
		ICommand RepairGameCommand { get; }
		ICommand CancelGameCommand { get; }
		ICommand ShowSettingsCommand { get; }
		ICommand ShowStatisticCommand { get; }

	}
}
