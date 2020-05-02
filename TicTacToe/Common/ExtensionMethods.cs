using System.Collections.Generic;

namespace Common
{
	public static class ExtensionMethods
	{
		public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> source)
		{
			foreach (var items in source)
				foreach (var item in items)
					yield return item;
		}
	}
}
