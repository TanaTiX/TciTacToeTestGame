using System;

namespace CommonUtils
{
	public class Utils
	{
		public static void Log(params object[] args)
		{
			var res = "";
			for (int i = 0; i < args.Length - 1; i++)
			{
				res += args[i].ToString() + " ";
			}
			res += args[args.Length - 1];
			Console.WriteLine(res);
		}
	}
}
