using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{

	public class UserDto : BaseId
	{
		public UserDto(int id, string userName, int imageIndex, int turn, bool isTurn) : base(id)
		{
			if (string.IsNullOrWhiteSpace(userName))
				throw new ArgumentNullException(nameof(userName));
			if (imageIndex < 0)
				throw new ArgumentNullException(nameof(imageIndex));

			UserName = userName;
			ImageIndex = imageIndex;
			Turn = turn;
			IsTurn = isTurn;
		}

		public string UserName { get; }
		public int ImageIndex { get; }
		public int Turn { get; }
		public bool IsTurn { get; }

	}


}
