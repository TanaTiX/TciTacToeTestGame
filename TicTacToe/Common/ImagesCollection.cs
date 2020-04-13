using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	public class ImagesCollection
	{
		//public static string Directory = @"Resources/Images/";
		public Dictionary<string, object> Pieces = new Dictionary<string, object>()
		{
			{"crossStandrart", @"Resources/Images/cross.png" },
			{"zeroStandrart", @"Resources/Images/zero.png" },
			{"crossYes", @"Resources/Images/yes.png" },
			{"zeroNo", @"Resources/Images/no.png" }
		};
		
	}
}
