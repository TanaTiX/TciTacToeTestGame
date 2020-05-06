using Common;
using ModelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Repo
{
	[Serializable]
	[XmlRoot("saveGameRoot")]
	public class SaveGame
	{
		public string FirstUser { get; set; }
		public string SecondUser { get; set; }
		public int ImageIndexFirstUser { get; set; }
		public int ImageIndexSecondUser { get; set; }
		public bool IsCurrentFirstUtser { get; set; }
		
		public List<CellXML> Cells { get; set; }
	}

	[Serializable]
	public class CellXML
	{
	
		public string CellType { get; set; }
		public int Column { get; set; }
		public int Row { get; set; }

		public CellXML()
		{

		}
		public CellXML(CellDto cell)
		{
			Row = cell.Row;
			Column = cell.Column;
			CellType = cell.CellType.ToString();
		}
		public CellDto CopyToDto()
		{

			return new CellDto(Column, Row, (CellContent)Enum.Parse(typeof(CellContent) , CellType));
		}
		public static List<CellXML> CreateCells(IEnumerable<IEnumerable<CellDto>> cells)
		{
			List<CellXML> list =cells.Concat().Select(cl => new CellXML(cl)).ToList();


			return list;
		}
	}
}
