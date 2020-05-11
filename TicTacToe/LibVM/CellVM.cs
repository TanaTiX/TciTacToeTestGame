using Common;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibVM
{
	public class CellVM : OnPropertyChangedClass
	{
		private CellTypeDto cellType;
		private int column;
		private int row;
		private int id;
		private string name;

		public CellTypeDto CellType { get => cellType; set => SetProperty(ref cellType, value); }
		public int Column { get => column; set => SetProperty(ref column, value); }
		public int Row { get => row; set => SetProperty(ref row, value); }
		public int Id { get => id; set => SetProperty(ref id, value); }
		//public string Name { get => name; set => SetProperty(ref name, value); }


	}
}
