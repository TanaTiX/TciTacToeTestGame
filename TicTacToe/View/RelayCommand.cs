using System;
using System.Windows.Input;

namespace View
{
	/// <summary>Реализация созданна из базовой https://metanit.com/sharp/wpf/22.3.php </summary>
	internal class RelayCommand : ICommand
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
			if (execute == null)
				throw new ArgumentNullException(nameof(execute));
			this.execute = execute;

			if (canExecute == null)
				this.canExecute = p => true;
			else
				this.canExecute = canExecute;
		}

		public bool CanExecute(object parameter) => canExecute(parameter);

		public void Execute(object parameter) => execute(parameter);
	}
}
