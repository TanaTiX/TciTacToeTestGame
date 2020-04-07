using System.Windows.Input;

namespace Common
{
	public class IGameVM
	{
		ICommand MoveCommand { get; }
		ICommand LoseCommand { get; }
	}
}
