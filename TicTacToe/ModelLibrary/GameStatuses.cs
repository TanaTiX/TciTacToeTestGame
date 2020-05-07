namespace ModelLibrary
{
	/// <summary>Перечисления статусов игры</summary>
	public enum GameStatuses 
	{
		/// <summary>Нулевая точка, значение по умолчанию</summary>
		Zero,
		/// <summary>Новая игра, 1й ход не сделан</summary>
		New,
		/// <summary>Идет игра</summary>
		Game,
		/// <summary>Победак</summary>
		Win,
		/// <summary>Победил 2й игрок</summary>
		//WinSecond, 
		/// <summary>Ничья</summary>
		Draw, 
		/// <summary>Отмена игры</summary>
		Cancel
	};
}
