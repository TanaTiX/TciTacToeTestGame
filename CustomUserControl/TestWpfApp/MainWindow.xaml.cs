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

namespace TestWpfApp
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		ViewModel viewModel;
		public MainWindow()
		{
			InitializeComponent();
			viewModel = (ViewModel)DataContext;
			viewModel.PropertyChanged += ViewModel_PropertyChanged;
		}

		private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			MessageBox.Show($"New Text=\"{viewModel.Text}\"");
		}

		private void StackPanel_Loaded(object sender, RoutedEventArgs e)
		{
			viewModel.Text = "Текст после загрузки";
		}
	}
}
