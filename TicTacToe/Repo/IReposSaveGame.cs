using ModelLibrary;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
public	interface IReposSaveGame
	{
		SavedGameDto Load();
		void Save(SavedGameDto gameDto);
	}
}
