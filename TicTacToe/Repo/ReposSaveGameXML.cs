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
		public Uri FileSaveXml { get; }

		public ReposSaveGameXML(Uri saveXML)
		{
			FileSaveXml = saveXML ?? throw new ArgumentNullException(nameof(saveXML));
		}
		public ReposSaveGameXML(string filePathSaveNameXml)
			//: this(new Uri(filePathNameXml, UriKind.RelativeOrAbsolute))
		{
			try
			{
				/// Попытка получения из строки Uri локального файла сохранения
				FileInfo fileSave = new FileInfo(filePathSaveNameXml);
				Uri uriSave = new Uri(fileSave.FullName, UriKind.Absolute);
				//Uri uri = new Uri(filePathNameXml, UriKind.RelativeOrAbsolute);
				if (!uriSave.IsFile)
					throw new ArgumentException("По пути в строке не удалось получить Uri локальнго файла", nameof(filePathSaveNameXml));
				FileSaveXml = uriSave;
			}
			catch (Exception)
			{
				/// Если не вышло, то надо по другому 
				/// интерпиритировать переданную строку.
				throw new ArgumentException("По строке не удалось получить Uri файла", nameof(filePathSaveNameXml));
			}
		}


		protected readonly XmlSerializer serializer = new XmlSerializer(typeof(SavedGameXML));



		public SavedGameDto Load()
		{
			SavedGameXML game;
			try
			{

				if (File.Exists(Path.GetFileName(FileSaveXml.LocalPath)))
				{
					using (var file = File.OpenRead(Path.GetFileName(FileSaveXml.LocalPath)))
						game = (SavedGameXML)serializer.Deserialize(file);
					return ConvertFromXml(game);
				}

			}
			catch (Exception) { }

			return null;
		}

		public void Save(SavedGameDto game)
		{
			//if (File.Exists(FileXml.OriginalString))
			//{
			//	File.Delete(FileXml.OriginalString);
			//}
			using (var file = File.Create(Path.GetFileName(FileSaveXml.LocalPath)))
				serializer.Serialize(file, ConvertFromDto(game));
		}

		protected static SavedGameDto ConvertFromXml(SavedGameXML game)
		{
			if (game == null)
				return null;

			if (!(game.CellTypes?.Count >= 3))
				throw new ArgumentOutOfRangeException(nameof(game) + "." + nameof(game.CellTypes), "Не может быть меньше трёх");

			HashSet<CellTypeDto> types = game.CellTypes
				.Select(xml => CellTypeDto.Create(xml.Id, xml.Value))
				.ToHashSet();

			var tps = types.ToDictionary(tp => tp.Id);

			HashSet<CellDto> cells = null;
			if (game.Cells != null)
			{
				cells = new HashSet<CellDto>();
				foreach (CellXML cell in game.Cells)
				{
					if (types.FirstOrDefault(dto => dto.Id == cell.TypeId) == null)
						throw new ArgumentException("Такого типа нет в списке", "cell.id");
					cells.Add(new CellDto(cell.Id, cell.Row, cell.Column, tps[cell.TypeId]));
				}
			}

			if (!(game.Users?.Count > 0))
				throw new ArgumentOutOfRangeException(nameof(game) + "." + nameof(game.Users), "Не может быть меньше одного");
			HashSet<UserDto> users = game.Users
					.Select(xml => new UserDto(xml.Id, xml.Name, xml.ImageIndex, xml.Turn, xml.Id == game.CurrentUser.UserId, tps[xml.TypeId]))
					.ToHashSet();

			return new SavedGameDto
			(
				users,
				cells,
				types,
				game.Game.Rows,
				game.Game.Columns,
				game.Game.LengthToWin
			);
		}
		protected static SavedGameXML ConvertFromDto(SavedGameDto game)
		{
			if (game == null)
				return null;

			//if (!(game.Types?.Count >= 3))
			if (!(game.Types != null && game.Types.Count >= 3))
				throw new ArgumentOutOfRangeException(nameof(game) + "." + nameof(game.Types), "Не может быть меньше трёх");

			List<CellTypeXML> types = game.Types
				.Select(dto => new CellTypeXML() { Id = dto.Id, Value = dto.Type })
				.ToList();

			var tps = types.ToDictionary(tp => tp.Value, tp => tp.Id);

			List<CellXML> cells = new List<CellXML>();
			if (game.Cells != null)
			{
				foreach (CellDto cell in game.Cells.Where(cll => cll?.CellType != null && cll.CellType != CellTypeDto.Empty))
				{
					cells.Add(new CellXML() { Id = cell.Id, Row = cell.Row, Column = cell.Column, TypeId = tps[cell.CellType.Type] });

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
			{
				throw new ArgumentOutOfRangeException(nameof(game) + "." + nameof(game.Users), "Не может быть меньше одного");
			}

			List<UserXML> users = game.Users
					.Select(dto => new UserXML() { Id = dto.Id, Name = dto.UserName, ImageIndex = dto.ImageIndex, Turn = dto.Turn, TypeId = tps[dto.CellType.Type] })
					.ToList();

			return new SavedGameXML()
			{
				Users = users,
				Cells = cells,
				CellTypes = types,
				CurrentUser = new CurrentUserXML() { UserId = turnId },
				Game = new GameSettings()
				{
					Rows = game.RowsCount,
					Columns = game.ColumnsCount,
					LengthToWin = game.LengthLineForWin
				}
			};

		}

		public void RemoveSavedGame()
		{
			if (File.Exists(FileSaveXml.AbsolutePath))
			{
				try
				{
					File.Delete(FileSaveXml.AbsolutePath);
				}
				catch (Exception ex)
				{

					throw ex;
				}
			}
		}
	}
}
