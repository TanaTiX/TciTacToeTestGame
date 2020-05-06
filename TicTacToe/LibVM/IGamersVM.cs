using Common;

namespace LibVM
{
	public interface IGamersVM
	{
		/// <summary>
		/// 1й пользователь
		/// </summary>
		Gamer FirstGamer { get; }
		/// <summary>
		/// 2й пользователь
		/// </summary>
		Gamer SecondGamer { get; }

	}
}
