using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Common
{
	public class RelayCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;
		
		private Action _execute;
		public RelayCommand(Action execute)
		{
			_execute = execute;
		}


		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			Console.WriteLine("test command " + parameter);
			_execute.Invoke();
		}
	}
}
