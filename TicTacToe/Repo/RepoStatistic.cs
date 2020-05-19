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

		protected static UserStatistic ConvertFromXml(StatisticGame stat)
		{
			if (stat == null)
			{
				return null;
			}

			HashSet<UserStatisticDto> users = stat.List.Select(xml => UserStatisticDto.Create(xml.Name, xml.Win, xml.Lose, xml.Draw)).ToHashSet();
			foreach (StatisticGameUser item in stat.List)
			{

			}
			//stat.List();
			return null;

			/*if (!(stat.cellTypes?.Count >= 3))
				throw new ArgumentOutOfRangeException(nameof(stat) + "." + nameof(stat.cellTypes), "Не может быть меньше трёх");

			HashSet<CellTypeDto> types = stat.cellTypes
				.Select(xml => CellTypeDto.Create(xml.id, xml.value))
				.ToHashSet();

			var tps = types.ToDictionary(tp => tp.Id);

			HashSet<CellDto> cells = null;
			if (stat.cells != null)
			{
				cells = new HashSet<CellDto>();
				foreach (CellXML cell in stat.cells)
				{
					if (types.FirstOrDefault(dto => dto.Id == cell.typeId) == null)
						throw new ArgumentException("Такого типа нет в списке", "cell.id");
					cells.Add(new CellDto(cell.id, cell.row, cell.column, tps[cell.typeId]));
				}
			}

			if (!(stat.users?.Count > 0))
				throw new ArgumentOutOfRangeException(nameof(stat) + "." + nameof(stat.users), "Не может быть меньше одного");
			HashSet<UserDto> users = stat.users
					.Select(xml => new UserDto(xml.id, xml.Name, xml.ImageIndex, xml.turn, xml.id == stat.currentUser.userId, tps[xml.typeId]))
					.ToHashSet();

			return new SavedGameDto
			(
				users,
				cells,
				types,
				stat.game.rows,
				stat.game.columns,
				stat.game.lengthToWin
			);*/
		}

		protected readonly XmlSerializer serializer = new XmlSerializer(typeof(StatisticGame));
		public UserStatistic LoadStatistic()
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

		public void SaveStatistic(UserDto[] gamers, bool isFirstWin, bool isSecondWin)
		{
			//throw new NotImplementedException();
			LoadStatistic();
		}

		SavedGameDto IReposStatistic.LoadStatistic()
		{
			throw new NotImplementedException();
		}
	}
}
