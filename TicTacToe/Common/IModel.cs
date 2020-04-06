using System.Collections.ObjectModel;

namespace Common
{
	public interface IModel
	{
		//event GameOverHandler GameOverWinEvent;
		//event GameOverDrawHandler GameOverDrawEvent;
		event MoveHandler MoveEvent;
		event ChangeStatusHandler ChangeStatusEvent;

		bool CanMove(CellDto cell, UserType user);
		bool Move(CellDto cell, UserType user);

		int RowsCount { get; }
		int ColumnsCount { get; }
		int LineLength { get; }
		UserType CurrentUser { get; }

		ReadOnlyCollection<ReadOnlyCollection<CellDto>> Cells { get; }
		GameStatuses GameStatus { get; }
	}
}
