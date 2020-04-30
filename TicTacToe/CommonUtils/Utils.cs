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
			
			//if (args[args.Length - 1] == null)
			//{
			//	res += "NULL";
			//}
			//else
			//{
				res += args[args.Length - 1].ToString();
			//}
			Console.WriteLine(res);
		}
	}
}
