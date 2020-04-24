using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Common
{
	public interface ISettingsVM
	{
		

		/// <summary>Вернуться на 1ю панель</summary>
		ICommand ExitSettingsCommand { get; }
		/// <summary>
		/// начало новой игры
		/// </summary>
		ICommand StartNewGameCommand { get; }
		/// <summary>
		/// 1й пользователь
		/// </summary>
		Gamer FirstGamer { get; }
		/// <summary>
		/// 2й пользователь
		/// </summary>
		Gamer SecondGamer { get; }
		/// <summary>
		/// список фишек для отображения
		/// </summary>
		IEnumerable<ImageSource> PiecesCollection { get; }

	}
}
