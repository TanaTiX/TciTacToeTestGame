using ModelLibrary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace LibVM
{
	public interface IGameVM :IGamersVM
	{
		/// <summary>Команда хода игрока</summary>
		ICommand MoveCommand { get; }

		/// <summary>Команда для принудительного проигрыша текущего игрока(сдался)</summary>
		ICommand LoseCommand { get; }

		/// <summary>Колоичество строк игрового поля</summary>
		int RowsCount { get; }

		/// <summary>Колоичество колонок игрового поля</summary>
		int ColumnsCount { get; }
		int LineLength { get; }

		/// <summary>Список ячеек игрвого поля</summary>
		ObservableCollection<CellVM> Cells { get; }
		ObservableCollection<CellTypeDto> CellTypes { get; }
		/// <summary>Словарь рисунков для контекста ячееек</summary>
		Dictionary<CellTypeDto, ImageSource> Picturies { get; }

		int CurrentUserIndex { get; }
		UserVM CurrentUser { get; }
	}
}
