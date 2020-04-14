using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CalculatorHistory
{
	public class Model
	{
		private readonly DataRootXML DataRoot;
		private readonly XmlSerializer Srl = new XmlSerializer(typeof(DataRootXML));
		private readonly Dictionary<string, string> Operators = new Dictionary<string, string>();

		public Model()
		{
			try
			{
				using (FileStream file = File.OpenRead("History.xml"))
					DataRoot = (DataRootXML)Srl.Deserialize(file);
				Operators = DataRoot.OperatorsType.ToDictionary(op => op.Title, op => op.Symbol);
			}
			catch (Exception ex)
			{
				DataRoot = new DataRootXML()
				{
					OperatorsType = new List<OperatorTypeXML>(),
					History = new List<ActionXML>()
				};
			}
		}

		public Dictionary<string, string> GetOperators()
			=> DataRoot.OperatorsType.ToDictionary(op => op.Title, op => op.Symbol);

		public IEnumerable<ActionXML> GetHistory()
			=> DataRoot.History.ToList();

		public void GetResult(ActionXML action)
		{
			if (!Operators.ContainsKey(action.Operator))
				throw new ArgumentException("Недопустимый оператор");

			switch(action.Operator)
			{
				case "Addition": action.Result = action.Left + action.Right; break;
				case "Subtraction": action.Result = action.Left - action.Right; break;
				case "Multiplication": action.Result = action.Left * action.Right; break;
				case "Division": action.Result = action.Left / action.Right; break;
			}

			DataRoot.History.Add(action);

			SaveAsync();
		}

		private async void SaveAsync()
		{
			await Task.Factory.StartNew(()=>
			{
				using (FileStream file = File.Create("History.xml"))
					Srl.Serialize(file, DataRoot);
			}
			);
		}
	}
}
