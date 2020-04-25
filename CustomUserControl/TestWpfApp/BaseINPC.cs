using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestWpfApp
{
	public abstract class BaseINPC : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string propertyName = "")
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		protected void Set<T>(ref T fieldProperty, T valueProperty, [CallerMemberName]string propertyName = "")
		{
			if (
					(fieldProperty==null && valueProperty != null)
					|| (fieldProperty != null && !fieldProperty.Equals(valueProperty))
				)
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
