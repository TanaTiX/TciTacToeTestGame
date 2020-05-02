﻿using Common;
using ModelLibrary;
using System;
using System.Linq;
using System.Runtime;

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
		}

		protected override void LoseMethod(object parameter)
		{
			base.LoseMethod(parameter);
			model.CancelGame();
		}
		protected override bool MoveCanMethod(object p)
		{
			return p is CellDto cell && model.CanMove(cell, CurrentUser);
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
			model.StartNewGame();
			UpdateCells();
			base.StartNewGameMethod(parameter);
			CurrentUser = model.CurrentUser;
		}

		
	}
}
