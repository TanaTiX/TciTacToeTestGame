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
	public class Gamer : OnPropertyChangedClass
	{
		private string _userName;
		private ImageSource _image;

		public string UserName { get => _userName; set => SetProperty(ref _userName , value); }
		public ImageSource Image { get => _image; set => SetProperty(ref  _image , value); }

		public Gamer Clone()
		{
			return new Gamer()
			{
				UserName = _userName,
				Image = _image
			};
		}

	}
}
