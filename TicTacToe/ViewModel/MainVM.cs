﻿using Common;
using ModelLibrary;
using System;
using System.Linq;
using System.Runtime;
using System.Windows.Controls;

namespace ViewModel
{
	public class MainVM : MainViewModel
	{
		protected Model model;
		public MainVM(Action<Type> windowsChanger, Model model)
			: base(windowsChanger, false)
		{
			this.model = model;
			RowsCount = model.RowsCount;
			ColumnsCount = model.ColumnsCount;
			//UpdateCells();

			CurrentUser = model.CurrentUser;
			model.ChangeStatusEvent += Model_ChangeStatusEvent;

			model.MoveEvent += Model_MoveEvent;
			IsRevenge = model.IsRevenge;

			

		}

		private void UpdateCells()
		{
			Cells.Clear();
			foreach (var cells in model.Cells)
				foreach (var cell in cells)
					Cells.Add(cell);
		}

		private void Model_MoveEvent(object sender, CellDto cell)
		{
			Cells[cell.Row * ColumnsCount + cell.Column] = cell;
			CurrentUser = model.CurrentUser;
		}

		private void Model_ChangeStatusEvent(object sender, GameStatuses status)
		{
			Statuse = model.GameStatus;
			CurrentUser = model.CurrentUser;
			IsRevenge = model.IsRevenge;
		}

		protected override void LoseMethod(object parameter)
		{
			base.LoseMethod(parameter);
			model.CancelGame();
		}
		protected override bool MoveCanMethod(object p)
		{
			bool ret =  p is CellDto cell && model.CanMove(cell, CurrentUser);
			return ret;
		}
		protected override void MoveMethod(object p)
		{
			if (p is CellDto cell) {
				model.Move(cell, CurrentUser);
				CurrentUser = model.CurrentUser;
			}
		}

		protected override void ShowStatisticMethod(object parameter)
		{
			//необходимо ерализовать в моделе
			base.ShowStatisticMethod(parameter);
		}


		protected override void StartNewGameMethod(object parameter)
		{
			model.CreateGame(FirstGamer, SecondGamer);
			model.StartNewGame();
			UpdateCells();
			base.StartNewGameMethod(parameter);
			CurrentUser = model.CurrentUser;
		}

		protected override void RepairGameMethod(object parameter)
		{
			SaveGame saveGame = model.Load(FirstGamer, SecondGamer);
			//model.StartNewGame();
			UpdateCells();
			//model.CreateGame(FirstGamer, SecondGamer);
			try
			{
				//for (int i = 0; i < saveGame.Cells.Count(); i++)
				//{
				//	CellContent type;
				//	string xmlType = saveGame.Cells[i].CellType;
				//	switch (xmlType)
				//	{
				//		case "Cross": type = CellContent.Cross; break;
				//		case "Zero": type = CellContent.Zero; break;
				//		case "Empty": type = CellContent.Empty; break;
				//		default: throw new Exception("При попытке загрузки сохраненной игры возникла ошибка");
				//	}
				//	//model.Cells = new CellDto(Cells[i].Column, Cells[i].Row, type);
				//	Cells[i] = new CellDto(Cells[i].Column, Cells[i].Row, type);
				//}

				//FirstGamer.UserName = model.FirstGamer.UserName;
				//SecondGamer.UserName = model.SecondGamer.UserName;
				//FirstGamer.ImageIndex = model.FirstGamer.ImageIndex;
				//SecondGamer.ImageIndex = model.SecondGamer.ImageIndex;

				FirstGamer.Image = PiecesCollection.ElementAt(FirstGamer.ImageIndex);
				SecondGamer.Image = PiecesCollection.ElementAt(SecondGamer.ImageIndex);
				/*model.FirstGamer.Image*/// = Picturies.ToArray();
										  //[model.FirstGamer.ImageIndex];

			}
			catch (Exception ex) { throw ex; }
			base.StartNewGameMethod(parameter);
			/*if (saveGame.IsCurrentFirstUtser)
			{
				model.CurrentUser = UserType.UserFirst;
			}
			else
			{
				model.CurrentUser = UserType.UserSecond;
			}*/
		}

	}
}
