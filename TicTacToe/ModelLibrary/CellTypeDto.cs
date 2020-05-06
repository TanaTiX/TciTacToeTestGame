using System;

namespace ModelLibrary
{
	public class CellTypeDto : BaseId
	{
		public CellTypeDto(int id, string type)
			: base(id)
		{
			if (string.IsNullOrWhiteSpace(Type))
				throw new ArgumentNullException(nameof(type));
			Type = type;
		}
		public string Type { get; }
	}
}
