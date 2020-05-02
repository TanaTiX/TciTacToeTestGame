using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Common
{
	public class Gamer : OnPropertyChangedClass, ICloneable
	{
		private string _userName;
		private ImageSource _image;
		private bool _isWin;

		public string UserName { get => _userName; set => SetProperty(ref _userName, value); }
		public ImageSource Image { get => _image; set => SetProperty(ref _image, value); }
		public int ImageIndex { get; set; }
		public bool IsWin { get => _isWin; set => SetProperty(ref _isWin, value); }

		//public Gamer Clone()
		//{
		//	return new Gamer()
		//	{
		//		UserName = _userName,
		//		Image = _image
		//	};
		//}

		public object Clone() => MemberwiseClone();
	}
}
