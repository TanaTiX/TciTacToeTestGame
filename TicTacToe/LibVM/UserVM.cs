using Common;
using ModelLibrary;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LibVM
{

	public class UserVM : OnPropertyChangedClass
	{
		private string _userName;
		private ImageSource _image;
		private bool _isWin;
		private int _turn;
		private bool _isTurn;
		private CellTypeDto _cellType;
		private int _imageIndex;

		public string UserName { get => _userName; set => SetProperty(ref _userName, value); }
		public ImageSource Image { get => _image; set => SetProperty(ref _image, value); }
		public int ImageIndex { get => _imageIndex; set => SetProperty(ref _imageIndex, value); }
		public bool IsWin { get => _isWin; set => SetProperty(ref _isWin, value); }

		public int Turn { get => _turn; set => SetProperty(ref _turn, value); }
		public bool IsTurn { get => _isTurn; set => SetProperty(ref _isTurn, value); }
		public CellTypeDto CellType { get => _cellType; set => SetProperty(ref _cellType, value); }
		private int _id;
		public int Id { get => _id; set => SetProperty(ref _id, value); }

	}


}
