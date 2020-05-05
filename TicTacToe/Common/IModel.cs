using System.Collections.ObjectModel;

namespace Common
{
	/// <summary>Интерфейс Модели</summary>
	/// <remarks>Интерфей Модели игры в крестики-нолики произвольного размера поля</remarks>
	public interface IModel
	{
		/// <summary>Собыие хода</summary>
		/// <remarks>Происходит при измении любой ячейки</remarks>
		event MoveHandler MoveEvent;
		/// <summary>Событие изменеия статуса игры</summary>
		/// <remarks>Происходит после изменения свойства GameStatus</remarks>
		event ChangeStatusHandler ChangeStatusEvent;

		/// <summary>Метод проверки допустимости хода</summary>
		/// <param name="cell">Данные о ходе</param>
		/// <returns><see langword="true"/> - если ход допустим</returns>
		bool CanMove(CellDto cell, UserType user);

		/// <summary>Метод хода</summary>
		/// <param name="cell">Данные хода</param>
		/// <param name="user">Игрок сделавший ход</param>
		/// <remarks>Если ход допустим, то будет возбужденно событие MoveEvent,
		/// в противном случае - ничего не произойдёт.</remarks>
		void Move(CellDto cell, UserType user);

		/// <summary>Количество строк в поле Игры</summary>
		int RowsCount { get; }
		/// <summary>Количество колонок в поле Игры</summary>
		int ColumnsCount { get; }
		/// <summary>Минимальная длина непрерывной последовательности 
		/// отметок необходимых для выигрыша/проигрыша</summary>
		int LineLength { get; }

		/// <summary>Игрок кто должен делать ход</summary>
		UserType CurrentUser { get; }

		/// <summary>Поле Игры через оболочку только для чтения</summary>
		ReadOnlyCollection<ReadOnlyCollection<CellDto>> Cells { get; }

		/// <summary>Статус игры</summary>
		GameStatuses GameStatus { get; }

		/// <summary>Отменить игру</summary>
		void CancelGame();

		/// <summary>Сохранить игру</summary>
		//void Save();

		///// <summary>Загрузить настройки и ранее сохраненную игру (при наличии)</summary>
		//void Load();

		bool IsRevenge { get; }
	}
}
