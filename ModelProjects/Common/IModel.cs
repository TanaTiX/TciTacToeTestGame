using Common;

namespace ModelLibrary
{
	public interface IModel
	{
		event GameOverHandler GameOverHandler;
		event MoveHandler MoveHandler;

		bool CanMove(CellDto cell);
		void Move(CellDto cell);
		int RowsCount { get; }
		int ColumnsCount { get; }
	}
}
