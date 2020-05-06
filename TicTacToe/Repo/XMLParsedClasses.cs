using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Repo
{

	// Примечание. Для запуска созданного кода может потребоваться NET Framework версии 4.5 или более поздней версии и .NET Core или Standard версии 2.0 или более поздней.
	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	[XmlRoot(Namespace = "", IsNullable = false, ElementName = "saveGame")]
	public partial class SavedGameXML
	{

		/// <remarks/>
		[XmlElement("user")]
		public List<UserXML> users { get; set; }

		/// <remarks/>
		[XmlElement("currentUser")]
		public CurrentUserXML currentUser { get; set; }

		/// <remarks/>
		[XmlElement("cell")]
		public List<CellXML> cells { get; set; }

		/// <remarks/>
		[XmlElement("cellType")]
		public List<CellTypeXML> cellTypes { get; set; }

		/// <remarks/>
		public GameSettings game { get; set; }
	}

	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	public partial class UserXML
	{

		/// <remarks/>
		[XmlAttribute()]
		public int id { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int turn { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public string Name { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int ImageIndex { get; set; }
	}

	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	public partial class CurrentUserXML
	{

		/// <remarks/>
		[XmlAttribute()]
		public int userId { get; set; }
	}

	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	public partial class CellXML
	{

		/// <remarks/>
		[XmlAttribute()]
		public int id { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int typeId { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int column { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int row { get; set; }
	}

	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	public partial class CellTypeXML
	{

		/// <remarks/>
		[XmlAttribute()]
		public int id { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public string value { get; set; }
	}

	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	public partial class GameSettings
	{

		/// <remarks/>
		[XmlAttribute()]
		public int columns { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int rows { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int lengthToWin { get; set; }
	}


}
