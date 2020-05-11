using Common;
using ModelLibrary;

namespace LibVM
{
	public interface IGamersVM
	{
		/// <summary>
		/// 1й пользователь
		/// </summary>
		UserVM FirstGamer { get; }
		/// <summary>
		/// 2й пользователь
		/// </summary>
		UserVM SecondGamer { get; }

	}
}
