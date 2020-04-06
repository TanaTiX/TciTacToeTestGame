using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugConsole
{
	public class CellView:CellDto
	{
		public CellView(int x, int y, CellContent content = CellContent.Empty):base(x, y, content)
		{

		}
	}
}
