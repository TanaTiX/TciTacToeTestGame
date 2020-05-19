namespace Repo
{

	// Примечание. Для запуска созданного кода может потребоваться NET Framework версии 4.5 или более поздней версии и .NET Core или Standard версии 2.0 или более поздней.
	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
	public partial class StatisticGame
	{

		private StatisticGameUser[] listField;

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayItemAttribute("User", IsNullable = false)]
		public StatisticGameUser[] List
		{
			get
			{
				return this.listField;
			}
			set
			{
				this.listField = value;
			}
		}
	}

	/// <remarks/>
	[System.SerializableAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	public partial class StatisticGameUser
	{

		private string nameField;

		private byte winField;

		private byte loseField;

		private byte drawField;

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public string Name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public byte Win
		{
			get
			{
				return this.winField;
			}
			set
			{
				this.winField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public byte Lose
		{
			get
			{
				return this.loseField;
			}
			set
			{
				this.loseField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlAttributeAttribute()]
		public byte Draw
		{
			get
			{
				return this.drawField;
			}
			set
			{
				this.drawField = value;
			}
		}
	}


}
