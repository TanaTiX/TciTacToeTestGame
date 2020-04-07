using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	public static class ImagesCollection
	{
		public static string Directoryn = @"/Images/";
		public static Dictionary<string, string> Pieces = new Dictionary<string, string>()
		{
			{"crossStandrart", @"cross.svg" },
			{"zeroStandrart", @"zero.svg" },
			{"crossYes", @"yes.svg" },
			{"zeroNo", @"no.svg" }
		};
		
	}
}
