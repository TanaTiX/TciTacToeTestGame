using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

		public RelayCommand ChangePieceIndexCommand { get; private set; }

		public MainViewModel()
		{
			ChangePieceIndexCommand = new RelayCommand(ShowTestCommand);
		}

		public void ShowTestCommand(object parameter)
		{
			MessageBox.Show("test");
		}
	}
}
