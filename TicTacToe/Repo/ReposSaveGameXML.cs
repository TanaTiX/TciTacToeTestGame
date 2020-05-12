using ModelLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Repo
{
	public class ReposSaveGameXML : IReposSaveGame
	{
		/// <summary>URI файла с данными</summary>
		public Uri FileXml { get; }

		public ReposSaveGameXML(Uri fileXml)
		{
			FileXml = fileXml ?? throw new ArgumentNullException(nameof(fileXml));
		}
		public ReposSaveGameXML(string filePathNameXml)
		{
			try
			{
				/// Попытка получения из строки Uri локального файла
				FileInfo file = new FileInfo(filePathNameXml);
				Uri uri = new Uri(file.FullName, UriKind.Absolute);
				//Uri uri = new Uri(filePathNameXml, UriKind.RelativeOrAbsolute);
				if (!uri.IsFile)
					throw new ArgumentException("По пути в строке не удалось получить Uri локальнго файла", nameof(filePathNameXml));
				FileXml = uri;
			}
			catch (Exception)
			{
				/// Если не вышло, то надо по другому 
				/// интерпиритировать переданную строку.

				throw new ArgumentException("По строке не удалось получить Uri файла", nameof(filePathNameXml));
			}

		}

		protected readonly XmlSerializer serializer = new XmlSerializer(typeof(SavedGameXML));



		public SavedGameDto Load()
		{
			SavedGameXML game;
			try
			{
				if (File.Exists(Path.GetFileName(FileXml.LocalPath)))
				{
					using (var file = File.OpenRead(Path.GetFileName(FileXml.LocalPath)))
						game = (SavedGameXML)serializer.Deserialize(file);
					return ConvertFromXml(game);
				}

			}
			catch (Exception) { }

			return null;
		}

		public void Save(SavedGameDto game)
		{
			using (var file = File.Create(Path.GetFileName(FileXml.LocalPath)))
				serializer.Serialize(file, ConvertFromDto(game));
		}

		protected static SavedGameDto ConvertFromXml(SavedGameXML game)
		{
			if (game == null)
				return null;

			if (!(game.cellTypes?.Count >= 3))
				throw new ArgumentOutOfRangeException(nameof(game) + "." + nameof(game.cellTypes), "Не может быть меньше трёх");

			HashSet<CellTypeDto> types = game.cellTypes
				.Select(xml => CellTypeDto.Create(xml.id, xml.value))
				.ToHashSet();

			var tps = types.ToDictionary(tp => tp.Id);

			HashSet<CellDto> cells = null;
			if (game.cells != null)
			{
				cells = new HashSet<CellDto>();
				foreach (CellXML cell in game.cells)
				{
					if (types.FirstOrDefault(dto => dto.Id == cell.typeId) == null)
						throw new ArgumentException("Такого типа нет в списке", "cell.id");
					cells.Add(new CellDto(cell.id, cell.row, cell.column, tps[cell.typeId]));
				}
			}

			if (!(game.users?.Count > 0))
				throw new ArgumentOutOfRangeException(nameof(game) + "." + nameof(game.users), "Не может быть меньше одного");
			HashSet<UserDto> users = game.users
					.Select(xml => new UserDto(xml.id, xml.Name, xml.ImageIndex, xml.turn, xml.id == game.currentUser.userId, tps[xml.typeId]))
					.ToHashSet();

			return new SavedGameDto
			(
				users,
				cells,
				types,
				game.game.rows,
				game.game.columns,
				game.game.lengthToWin
			);
		}
		protected static SavedGameXML ConvertFromDto(SavedGameDto game)
		{
			if (game == null)
				return null;

			if (!(game.Types?.Count >= 3))
				throw new ArgumentOutOfRangeException(nameof(game) + "." + nameof(game.Types), "Не может быть меньше трёх");

			List<CellTypeXML> types = game.Types
				.Select(dto => new CellTypeXML() { id = dto.Id, value = dto.Type })
				.ToList();

			var tps = types.ToDictionary(tp => tp.value, tp => tp.id);

			List<CellXML> cells = new List<CellXML>();
			if (game.Cells != null)
			{
				foreach (CellDto cell in game.Cells.Where(cll => cll?.CellType != null && cll.CellType != CellTypeDto.Empty))
				{
					cells.Add(new CellXML() { id = cell.Id, row = cell.Row, column = cell.Column, typeId = tps[cell.CellType.Type] });

				}
			}
			int turnId = -1;
			foreach (UserDto user in game.Users)
			{
				if (user.IsTurn)
				{
					turnId = user.Id;
					break;
				}
			}

			if (!(game.Users?.Count > 0))
				throw new ArgumentOutOfRangeException(nameof(game) + "." + nameof(game.Users), "Не может быть меньше одного");
			List<UserXML> users = game.Users
					.Select(dto => new UserXML() { id = dto.Id, Name = dto.UserName, ImageIndex = dto.ImageIndex, turn = dto.Turn, typeId = tps[dto.CellType.Type] })
					.ToList();

			return new SavedGameXML()
			{
				users = users,
				cells = cells,
				cellTypes = types,
				currentUser = new CurrentUserXML() { userId = turnId },
				game = new GameSettings()
				{
					rows = game.RowsCount,
					columns = game.ColumnsCount,
					lengthToWin = game.LengthLineForWin
				}
			};

		}

		public void RemoveSavedGame()
		{
			if (File.Exists(FileXml.AbsolutePath))
			{
				try
				{
					File.Delete(FileXml.AbsolutePath);
				}
				catch (Exception ex)
				{

					throw ex;
				}
			}
		}
	}
}
