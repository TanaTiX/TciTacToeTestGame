namespace Common
{
	/// <summary>
	/// Zero - нулевая точка, значение по умолчанию
	/// New - новая игра, 1й ход не сделан
	/// Game - идет игра
	/// Win - кто-то победил/проиграл
	/// Draw - ничья
	/// Cancel - отмена игры
	/// </summary>
	public enum GameStatuses { Zero, New, Game, Win, Draw, Cancel };
}
