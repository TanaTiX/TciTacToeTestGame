using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Common
{
    #region Делегаты для методов WPF команд
    public delegate void ExecuteHandler(object parameter);
    public delegate bool CanExecuteHandler(object parameter);
    #endregion

    #region Класс команд - RelayCommand
    /// <summary>Класс реализующий интерфейс ICommand для создания WPF команд</summary>
    public class RelayCommand : ICommand
    {
        private readonly CanExecuteHandler _canExecute;
        private readonly ExecuteHandler _onExecute;
        private readonly EventHandler _requerySuggested;
		private Action moveMethod;
		private Func<bool> moveCanMethod;

		/// <summary>Событие извещающее об изменении состояния команды</summary>
		public event EventHandler CanExecuteChanged;

        /// <summary>Конструктор команды</summary>
        /// <param name="execute">Выполняемый метод команды</param>
        /// <param name="canExecute">Метод разрешающий выполнение команды</param>
        public RelayCommand(ExecuteHandler execute, CanExecuteHandler canExecute = null)
        {
            _onExecute = execute;
            _canExecute = canExecute;

            _requerySuggested = (o, e) => Invalidate();
            CommandManager.RequerySuggested += _requerySuggested;
        }

		public RelayCommand(Action moveMethod, Func<bool> moveCanMethod)
		{
			this.moveMethod = moveMethod;
			this.moveCanMethod = moveCanMethod;
		}

		public void Invalidate()
            => Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }), null);

        /// <summary>Вызов разрешающего метода команды</summary>
        /// <param name="parameter">Параметр команды</param>
        /// <returns>True - если выполнение команды разрешено</returns>
        public bool CanExecute(object parameter) => _canExecute == null ? true : _canExecute.Invoke(parameter);

        /// <summary>Вызов выполняющего метода команды</summary>
        /// <param name="parameter">Параметр команды</param>
        public void Execute(object parameter) => _onExecute?.Invoke(parameter);
    }
    #endregion

    //public class RelayCommand : ICommand
    //{
    //	public event EventHandler CanExecuteChanged;

    //	private Action _execute;
    //	public RelayCommand(Action execute)
    //	{
    //		_execute = execute;
    //	}


    //	public bool CanExecute(object parameter)
    //	{
    //		return true;
    //	}

    //	public void Execute(object parameter)
    //	{
    //		Console.WriteLine("test command " + parameter);
    //		_execute.Invoke();
    //	}
    //}
}
