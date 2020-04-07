using System.Windows.Input;

namespace Common
{
	public interface IGameVM
	{
		/// <summary>Команда хода игрока</summary>
		ICommand MoveCommand { get; }

		/// <summary>Команда для принудительного проигрыша текущего игрока(сдался)</summary>
		ICommand LoseCommand { get; }
	}
}
