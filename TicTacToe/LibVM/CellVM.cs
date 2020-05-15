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
		private int _column;
		private int _row;
		private int _id;
		private string name;
		private CellTypeDto _cellType;
		public CellTypeDto CellType { get => _cellType; set => SetProperty(ref _cellType, value); }
		public int Column { get => _column; set => SetProperty(ref _column, value); }
		public int Row { get => _row; set => SetProperty(ref _row, value); }
		public int Id { get => _id; set => SetProperty(ref _id, value); }
		//public string Name { get => name; set => SetProperty(ref name, value); }


	}
}
