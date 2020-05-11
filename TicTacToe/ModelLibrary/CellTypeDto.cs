using System;

namespace ModelLibrary
{
	public class CellTypeDto : BaseId
	{
		public CellTypeDto(int id, string type)
			: base(id)
		{
			if (string.IsNullOrWhiteSpace(type))
				throw new ArgumentNullException(nameof(type));
			Type = type;
		}
		public string Type { get; }
		public static CellTypeDto Empty { get; } = new CellTypeDto(-1, "Empty");
		public static CellTypeDto Cross { get; } = new CellTypeDto(1, "Cross");
		public static CellTypeDto Zero { get; } = new CellTypeDto(2, "Zero");
	}
}
