using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CalculatorHistory
{

	// Примечание. Для запуска созданного кода может потребоваться NET Framework версии 4.5 или более поздней версии и .NET Core или Standard версии 2.0 или более поздней.
	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	[XmlRoot(Namespace = "", IsNullable = false, ElementName = "DataRoot")]
	public partial class DataRootXML
	{

		/// <remarks/>
		[XmlArrayItem("OperatorType", IsNullable = false)]
		public List<OperatorTypeXML> OperatorsType { get; set; }

		/// <remarks/>
		[XmlArrayItem("Action", IsNullable = false)]
		public List<ActionXML> History { get; set; }
	}

	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	public partial class OperatorTypeXML
	{

		/// <remarks/>
		[XmlAttribute()]
		public string Title { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public string Symbol { get; set; }
	}

	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	public partial class ActionXML
	{

		/// <remarks/>
		[XmlAttribute()]
		public decimal Left { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public string Operator { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public decimal Right { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public decimal Result { get; set; }
	}


}
