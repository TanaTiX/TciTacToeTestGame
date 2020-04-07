using System.Windows.Input;

namespace Common
{
	public interface ISettingsVM
	{
		/// <summary>Команда замены ник-нэйма пользователя</summary>
		ICommand ChangeUserNameCommand { get; }

		/// <summary>Команда смены отображаемых фишек, которыми играет пользователь</summary>
		ICommand ChangeUserPieceCommand { get; }
	}
}
