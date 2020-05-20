using ModelLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

		protected static UsersStatisticDto ConvertFromXml(StatisticGame stat)
		{
			if (stat == null)
			{
				return null;
			}

			//HashSet<UserStatisticDto> users = stat.List.Select(xml => UserStatisticDto.Create(xml.Name, xml.Win, xml.Lose, xml.Draw)).ToHashSet();
			//List<UserStatistic> userStatistics = new List<UserStatistic>();
			Dictionary<string, UserStatisticDto> statisticDictionary = new Dictionary<string, UserStatisticDto>();
			//HashSet<UserStatisticDto> userStatistics = new HashSet<UserStatisticDto>();
			foreach (StatisticGameUser item in stat.List)
			{
				statisticDictionary[item.Name] = UserStatisticDto.Create(item.Name, item.Win, item.Lose, item.Draw);
				//userStatistics.Add(UserStatisticDto.Create(item.Name, item.Win, item.Lose, item.Draw));
			}
			//stat.List();
			return new UsersStatisticDto(statisticDictionary);

		}

		protected readonly XmlSerializer serializer = new XmlSerializer(typeof(StatisticGame));
		
		public UsersStatisticDto LoadStatistic()
		{
			StatisticGame statistic;
			try
			{

				if (File.Exists(Path.GetFileName(FileStatisticXml.LocalPath)))
				{
					using (var file = File.OpenRead(Path.GetFileName(FileStatisticXml.LocalPath)))
						statistic = (StatisticGame)serializer.Deserialize(file);
					return ConvertFromXml(statistic);
				}

			}
			catch (Exception) { }

			return null;
		}

		private void UpdateValue(Dictionary<string, UserStatisticDto> dict, string userName, int win, int lose, int draw)
		{
			if (dict.TryGetValue(userName, out _))
			{
				dict[userName] = UserStatisticDto.Create(
					userName,
					dict[userName].Win + win,
					dict[userName].Lose + lose,
					dict[userName].Draw + draw);
			}
			else
			{
				dict[userName] = UserStatisticDto.Create(
					userName,
					win,
					lose,
					draw);
			}
		}

		public void SaveStatistic(UserDto[] gamers, bool isFirstWin, bool isSecondWin)
		{
			UsersStatisticDto statistic = LoadStatistic();
			UserDto first = gamers[0];
			UserDto second = gamers[1];
			if (isFirstWin || isSecondWin)
			{

				if (isFirstWin)
				{
					UpdateValue(statistic.UsersStatistic, first.UserName, 1, 0, 0);
					UpdateValue(statistic.UsersStatistic, second.UserName, 0, 1, 0);
				}
				else
				{
					UpdateValue(statistic.UsersStatistic, second.UserName, 1, 0, 0);
					UpdateValue(statistic.UsersStatistic, first.UserName, 0, 1, 0);
				}
			}
			else
			{
				UpdateValue(statistic.UsersStatistic, second.UserName, 0, 0, 1);
				UpdateValue(statistic.UsersStatistic, first.UserName, 0, 0, 1);
			}
			
			StatisticGame statisticGame = new StatisticGame();
			statisticGame.List = statistic.UsersStatistic.Values.Select(u => new StatisticGameUser() { Name = u.Name, Win = u.Win, Lose = u.Lose, Draw = u.Draw }).ToArray();
			
			using (var file = File.Create(Path.GetFileName(FileStatisticXml.LocalPath)))
				serializer.Serialize(file, statisticGame);
		}

	}
}
