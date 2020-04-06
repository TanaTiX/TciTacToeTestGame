namespace Common
{
	//public delegate void GameOverHandler(object sender);
	//public delegate void GameOverDrawHandler(object sender);
	public delegate void MoveHandler(object sender, CellDto cell);
	public delegate void ChangeStatusHandler(object sender, GameStatuses status);
}