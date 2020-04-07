using System.Windows.Input;

namespace Common
{
	public class ISettingsVM
	{
		ICommand ChangeUserNameCommand { get; }
		ICommand ChangeUserPieceCommand { get; }
	}
}
