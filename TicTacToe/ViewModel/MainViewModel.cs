using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class MainViewModel: OnPropertyChangedClass
	{
        public string Test = "jhvkhv";
		public Dictionary<string, object> Pieces { get; } = new Dictionary<string, object>()
		{
			{"crossStandrart", @"Resources/Images/cross.png" },
			{"zeroStandrart", @"Resources/Images/zero.png" },
			{"crossYes", @"Resources/Images/yes.png" },
			{"zeroNo", @"Resources/Images/no.png" }
		};
	}
}
