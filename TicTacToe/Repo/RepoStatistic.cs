using ModelLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo
{
	public class RepoStatistic : IReposStatistic
	{
		
		public Uri FileStatisticXml { get; }

		public RepoStatistic(Uri statisticXML)
		{
			FileStatisticXml = statisticXML ?? throw new ArgumentNullException(nameof(statisticXML));
		}
		public RepoStatistic(string filePathStatisticXML)
		{
			try
			{
				/// Попытка получения из строки Uri локального файла статистики
				FileInfo fileStatistic = new FileInfo(filePathStatisticXML);
				Uri uriStatistic = new Uri(fileStatistic.FullName, UriKind.Absolute);
				if (!uriStatistic.IsFile)
					throw new ArgumentException("По пути в строке не удалось получить Uri локальнго файла", nameof(filePathStatisticXML));
				FileStatisticXml = uriStatistic;

			}
			catch (Exception)
			{
				/// Если не вышло, то надо по другому 
				/// интерпиритировать переданную строку.
				throw new ArgumentException("По строке не удалось получить Uri файла", nameof(filePathStatisticXML));
			}
		}
		public SavedGameDto LoadStatistic()
		{
			throw new NotImplementedException();
		}

		public void SaveStatistic(UserDto[] gamers, bool isFirstWin, bool isSecondWin)
		{
			throw new NotImplementedException();
		}
	}
}
