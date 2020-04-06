using System.Collections.ObjectModel;

namespace Common
{
	public interface IModel
	{
		event GameOverHandler GameOverWinEvent;
		event GameOverDrawHandler GameOverDrawEvent;
		event MoveHandler MoveEvent;

		bool CanMove(CellDto cell);
		void Move(CellDto cell);

		int RowsCount { get; }
		int ColumnsCount { get; }
		int LineLength { get; }

		ReadOnlyCollection<ReadOnlyCollection<CellDto>> Cells { get; }
	}
}
