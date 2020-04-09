using System.Windows.Input;

namespace Common
{
	public interface IGameVM
	{
		/// <summary>Команда хода игрока</summary>
		ICommand MoveCommand { get; }

		/// <summary>Команда для принудительного проигрыша текущего игрока(сдался)</summary>
		ICommand LoseCommand { get; }

		/// <summary>Колоичество строк игрового поля</summary>
		int RowsCount { get; }

		/// <summary>Колоичество колонок игрового поля</summary>
		int ColumnsCount { get; }

		/// <summary>Список ячеек игрвого поля</summary>
		CellDto[][] Cells { get; }
	}
}
