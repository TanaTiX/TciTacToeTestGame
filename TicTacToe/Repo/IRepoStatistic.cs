using ModelLibrary;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
	public interface IReposStatistic
	{
		SavedGameDto LoadStatistic();
		void SaveStatistic(UserDto[] gamers, bool isFirstWin, bool isSecondWin);
	}
}
