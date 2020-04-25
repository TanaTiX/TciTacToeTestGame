using System;
using System.Windows.Input;

namespace TestWpfApp
{
	public class RelayCommand : ICommand
	{
		private readonly Action<object> execute;
		private readonly Func<object, bool> canExecute;

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			this.execute = execute ?? throw new ArgumentNullException(nameof(execute));

			this.canExecute = canExecute ?? CanExecuteDefault;
		}

		public bool CanExecute(object parameter) => canExecute(parameter);

		public void Execute(object parameter) => execute(parameter);

		private static bool CanExecuteDefault(object parameter) => true;
	}
}
