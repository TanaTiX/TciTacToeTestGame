using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelLibrary
{
	public class CellTypeDto : BaseId
	{
		private CellTypeDto(int id, string type)
			: base(id)
		{
			if (string.IsNullOrWhiteSpace(type))
				throw new ArgumentException("Is null or white space", nameof(type));
			Type = type;
		}
		public string Type { get; }
		public static CellTypeDto Empty { get; } = new CellTypeDto(-1, "Empty");
		public static CellTypeDto Cross { get; } = new CellTypeDto(1, "Cross");
		public static CellTypeDto Zero { get; } = new CellTypeDto(2, "Zero");

		private static readonly Dictionary<string, CellTypeDto> CellTypes
			= new Dictionary<string, CellTypeDto>(new CellTypeDtoKeyComparer()) { { Empty.Type, Empty }, { Cross.Type, Cross }, { Zero.Type, Zero } };


		private static readonly HashSet<int> AllId = new HashSet<int>() { Empty.Id, Cross.Id, Zero.Id };

		private class CellTypeDtoKeyComparer : IEqualityComparer<string>
		{
			public bool Equals(string x, string y)
			{
				return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
			}

			public int GetHashCode(string obj)
			{
				if (obj == null)
					return "string.null".GetHashCode();

				if (string.IsNullOrEmpty(obj))
					return "string.Empty".GetHashCode();

				if (string.IsNullOrWhiteSpace(obj))
					return "string.WhiteSpace".GetHashCode();

				return obj.ToLower().GetHashCode();
			}
		}

		public static CellTypeDto Create(string type)
			=> Create(-10, type);

		public static CellTypeDto Create(int id, string type)
		{
			if (string.IsNullOrWhiteSpace(type))
				throw new ArgumentException("Is null or white space", nameof(type));

			lock (CellTypes)
			{
				if (CellTypes.TryGetValue(type, out CellTypeDto cellType))
					return cellType;

				lock (AllId)
				{
					if (id <= 0 || AllId.Contains(id))
						for (id = 1; AllId.Contains(id); id++)
						{ }

					type = char.ToUpper(type[0]) + type.Substring(1);

					cellType = new CellTypeDto(id, type);

					CellTypes.Add(type, cellType);
					AllId.Add(id);

					return cellType;
				}
			}
		}

	}
}
