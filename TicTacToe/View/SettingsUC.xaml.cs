using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using View.Properties;

namespace View
{
	/// <summary>
	/// Логика взаимодействия для SettingsUC.xaml
	/// </summary>
	public partial class SettingsUC : UserControl
	{
		public SettingsUC()
		{
			InitializeComponent();
			if (!(DataContext is ISettingsVM viewModel))
			{
				 viewModel= new SettingsVM()
				{
					PiecesCollection = (IEnumerable<ImageSource>)Resources["Images"]
				};

				viewModel.FirstGamer.UserName = "11111111111111";
				User1.ItemsSource = viewModel.PiecesCollection;
				DataContext = viewModel;
			}
		}
	}
}
