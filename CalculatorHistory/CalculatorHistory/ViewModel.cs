using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Common;

namespace CalculatorHistory
{
	public class ViewModel : OnPropertyChangedClass
	{
		private decimal _left;
		private string _operator;
		private decimal _right;
		private decimal _result;

		public decimal Left { get => _left; set => SetProperty(ref _left, value); }
		public string Operator { get => _operator; set => SetProperty(ref _operator, value); }
		public decimal Right { get => _right; set => SetProperty(ref _right, value); }
		public decimal Result { get => _result; set => SetProperty(ref _result, value); }

		public Dictionary<string, string> Operators { get; }

		private readonly Model model = new Model();

		public ObservableCollection<ActionXML> History { get; } = new ObservableCollection<ActionXML>();

		public ViewModel()
		{
			Operators = model.GetOperators();
			if (Operators?.Keys.Count > 0)
				Operator = Operators.Keys.First();
			foreach (ActionXML action in model.GetHistory())
				History.Add(action);

			GetResultCommand = new RelayCommand(GetResultMethod, GetResultCanMethod);
		}
		public RelayCommand GetResultCommand { get; }

		private bool GetResultCanMethod(object parameter)
			=> !string.IsNullOrWhiteSpace(Operator) && Operators.ContainsKey(Operator);

		private void GetResultMethod(object parameter)
		{
			ActionXML action = new ActionXML()
			{
				Left = Left,
				Operator = Operator,
				Right = Right
			};

			model.GetResult(action);
			Result = action.Result;

			History.Add(action);
		}

		public Dictionary<string, string> Images { get; } = new Dictionary<string, string>()
			{
				{"Addition" , @"Images\plus.png"},
				{"Subtraction" ,@"Images\subt.png"},
				{"Multiplication" ,@"Images\multi.png"},
				{"Division" ,@"Images\div.png"}
			};

	}
}
