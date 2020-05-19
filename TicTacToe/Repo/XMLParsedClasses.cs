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
		public List<UserXML> Users { get; set; }

		/// <remarks/>
		[XmlElement("currentUser")]
		public CurrentUserXML CurrentUser { get; set; }

		/// <remarks/>
		[XmlElement("cell")]
		public List<CellXML> Cells { get; set; }

		/// <remarks/>
		[XmlElement("cellType")]
		public List<CellTypeXML> CellTypes { get; set; }

		/// <remarks/>
		public GameSettings Game { get; set; }
	}

	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	public partial class UserXML
	{

		/// <remarks/>
		[XmlAttribute()]
		public int Id { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int Turn { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public string Name { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int ImageIndex { get; set; }
		/// <remarks/>
		[XmlAttribute()]
		public int TypeId { get; set; }

	}

	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	public partial class CurrentUserXML
	{

		/// <remarks/>
		[XmlAttribute()]
		public int UserId { get; set; }
	}

	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	public partial class CellXML
	{

		/// <remarks/>
		[XmlAttribute()]
		public int Id { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int TypeId { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int Column { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int Row { get; set; }
	}

	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	public partial class CellTypeXML
	{

		/// <remarks/>
		[XmlAttribute()]
		public int Id { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public string Value { get; set; }
	}

	/// <remarks/>
	[Serializable()]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	public partial class GameSettings
	{

		/// <remarks/>
		[XmlAttribute()]
		public int Columns { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int Rows { get; set; }

		/// <remarks/>
		[XmlAttribute()]
		public int LengthToWin { get; set; }
	}


}
