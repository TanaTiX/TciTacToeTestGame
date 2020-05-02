using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace View
{
	/// <summary>
	/// Логика взаимодействия для UserWinnerGameEndUC.xaml
	/// </summary>
	public partial class UserGameEndUC : UserGameEndUCBase
	{
		public UserGameEndUC()
		{
			InitializeComponent();
		}





	}

	public partial class UserGameEndUCBase : UserControl
	{		public string Title
		{
			get { return (string)GetValue(NameGamerProperty); }
			set { SetValue(NameGamerProperty, value); }
		}

		// Using a DependencyProperty as the backing store for NameGamer.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty NameGamerProperty =
			DependencyProperty.Register(nameof(Title), typeof(string), typeof(UserGameEndUC), new PropertyMetadata(null));

	}
}
