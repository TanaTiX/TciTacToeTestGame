using System;

namespace ModelLibrary
{
	public class BaseId : IEquatable<BaseId>
	{
		public BaseId(int id)
		{
			Id = id;
		}

		public int Id { get; }

		public override bool Equals(object obj)
		{
			return obj is BaseId other && Equals(other);
		}

		public bool Equals(BaseId other)
		{
			return Id == other.Id;
		}

		public override int GetHashCode()
		{
			return 2108858624 + Id.GetHashCode();
		}

	}
}
