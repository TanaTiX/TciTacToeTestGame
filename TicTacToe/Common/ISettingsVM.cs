using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Common
{
	public interface ISettingsVM : IGamersVM
	{
		

		/// <summary>Вернуться на 1ю панель</summary>
		ICommand ShowFirstScreenCommand { get; }
		/// <summary>
		/// начало новой игры
		/// </summary>
		ICommand StartNewGameCommand { get; }
		/// <summary>
		/// список фишек для отображения
		/// </summary>
		IEnumerable<ImageSource> PiecesCollection { get; }

	}
}
