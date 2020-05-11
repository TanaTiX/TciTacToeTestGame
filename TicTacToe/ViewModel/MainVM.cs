using Common;
using LibVM;
using Model;
using ModelLibrary;
using Repo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime;
using System.Windows.Controls;
using System.Windows.Media;

namespace ViewModel
{
	public class MainVM : MainViewModel
	{
		protected IModel model;
		public MainVM(Action<Type> windowsChanger, IModel model)
			: base(windowsChanger, false)
		{
			this.model = model;
			//RowsCount = model.RowsCount;
			//ColumnsCount = model.ColumnsCount;
			//	IsRevenge = model.IsRevenge;
			//UpdateCells();

			//CurrentUser = model.CurrentUser;
			//model.ChangeStatusEvent += Model_ChangeStatusEvent;
			//model.PropertyChanged += OnModelPropertyChanged;
			//model.ChangedCellEvent += OnModelMoveEvent;
			model.ChangedStateEvent += Model_ChangedStateEvent;

			//model.OnAllPropertyChanged();


		}

		public MainVM(Action<Type> windowsChanger, IModel model, int rows, int columns, int length)
			: this(windowsChanger, model)
		{
			RowsCount = rows;
			ColumnsCount = columns;
			LineLength = length;
		}

		private static void CopyPropertiesToVM(UserDto user, UserVM userVM)
		{
			userVM.CellType = user.CellType;
			userVM.ImageIndex = user.ImageIndex;
			userVM.IsTurn = user.IsTurn;
			userVM.Turn = user.Turn;
			userVM.UserName = user.UserName;
			userVM.Id = user.Id;
		}

		private static UserDto ConvertVMToDto(UserVM user)
		{
			return new UserDto
			(
				user.Id,
				user.UserName,
				user.ImageIndex,
				user.Turn,
				user.IsTurn,
				user.CellType
			);
		}
		private static CellDto ConvertVMToDto(CellVM cell
			)
		{
			return new CellDto
			(
				cell.Id,
				cell.Row,
				cell.Column,
				cell.CellType
			//cell.Name
			);
		}

		private void Model_ChangedStateEvent(object sender, ChangedStateHandlerArgs e)
		{

			switch (e.StateName)
			{
				case NamesState.RowsCount:
					RowsCount = (int)e.NewValue;
					break;
				case NamesState.ColumnsCount:
					ColumnsCount = (int)e.NewValue;
					break;
				case NamesState.LineLength:
					LineLength = (int)e.NewValue;
					break;
				case NamesState.GameStatus:
					//int userId = (int)e.NewValue[1];

					if (e.NewValue is GameStatuses status)
					{
						Statuse = status;
					}
					else
					{
						object[] args = (object[])e.NewValue;
						int userId = (int)args[1];
						status = (GameStatuses)args[0];
						if (status == GameStatuses.Win)
						{
							if (FirstGamer.Id == userId)
							{
								Winner = FirstGamer;
								Loser = SecondGamer;
							}
							else if (SecondGamer.Id == userId)
							{
								Winner = SecondGamer;
								Loser = FirstGamer;
							}
							else
							{
								throw new ArgumentException("Не существующий UserId, когда один из игроков сдался/выиграл");
							}
						}
						else
						{
							throw new ArgumentException("Не допустимый статус игры");
						}
						Statuse = status;
					}
					break;
				case NamesState.Gamers:
					var users = ((IEnumerable<UserDto>)e.NewValue).OrderBy(us => us.Turn);
					var first = users.First();
					var second = users.Skip(1).First();

					CopyPropertiesToVM(first, FirstGamer);
					CopyPropertiesToVM(second, SecondGamer);
					FirstGamer.Image = PiecesCollection.ElementAt(FirstGamer.ImageIndex);
					SecondGamer.Image = PiecesCollection.ElementAt(SecondGamer.ImageIndex);
					break;
				case NamesState.Types:
					CellTypes.Clear();
					((ISet<CellTypeDto>)e.NewValue).ToList().ForEach(c => CellTypes.Add(c));
					FirstGamer.CellType = CellTypes[1];
					SecondGamer.CellType = CellTypes[2];
					break;
				case NamesState.CurrentGamerId:
					break;
				case NamesState.CurrentGamerIndex:
					CurrentUserIndex = (int)e.NewValue;
					break;
				case NamesState.CurrentGamer:
					var user = (UserDto)e.NewValue;
					if (FirstGamer.UserName == user.UserName)
						CurrentUserIndex = 0;
					else if (SecondGamer.UserName == user.UserName)
						CurrentUserIndex = 1;
					else
						CurrentUserIndex = -1;
					break;
				case NamesState.IsRevenge:
					IsRevenge = (bool)e.NewValue;
					break;
				case NamesState.CellType:
					CellDto cell = (CellDto)e.NewValue;
					cellsMatrix[cell.Row, cell.Column].CellType = cell.CellType;
					break;
				case NamesState.ChangeCellsCount:
					int[] cellsParams = (int[])e.NewValue;
					RowsCount = cellsParams[0];
					ColumnsCount = cellsParams[1];
					Cells.Clear();
					cellsMatrix = new CellVM[RowsCount, ColumnsCount];
					for (int row = 0; row < RowsCount; row++)
					{
						for (int col = 0; col < ColumnsCount; col++)
						{
							Cells.Add(cellsMatrix[row, col] = new CellVM() { Row = row, Column = col });
						}
					}
					break;
				case NamesState.ChangeGamerIsTurn:
					UserDto newUser = (UserDto)e.NewValue;
					UserDto oldUser = (UserDto)e.OldValue;
					UserVM newUserVM = FirstGamer, oldUserVM = SecondGamer;
					if (FirstGamer.UserName != newUserVM.UserName)
						(newUserVM, oldUserVM) = (oldUserVM, newUserVM);
					newUserVM.IsTurn = true;
					oldUserVM.IsTurn = false;
					break;
				default:
					break;
			}
		}

		//private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		//{

		//	if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == nameof(Model.ModelTicTacToe.GameStatus))
		//	{
		//		Statuse = model.GameStatus;
		//		if(Statuse == GameStatuses.Game)
		//		{
		//			UpdateCells();
		//		}
		//	}

		//	if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == nameof(Model.ModelTicTacToe.RowsCount))
		//		RowsCount = model.RowsCount;
		//	if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == nameof(Model.ModelTicTacToe.ColumnsCount))
		//		ColumnsCount = model.ColumnsCount;
		//	if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == nameof(Model.ModelTicTacToe.IsRevenge))
		//		IsRevenge = model.IsRevenge;
		//	if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == nameof(Model.ModelTicTacToe.CurrentUser))
		//		CurrentUser = model.CurrentUser;

		//}

		//private void UpdateCells()
		//{
		//	Cells.Clear();
		//	foreach (var cells in model.Cells)
		//		foreach (var cell in cells)
		//			Cells.Add(cell);
		//}

		//private void OnModelMoveEvent(object sender, CellDto cell)
		//{
		//	Cells[cell.Row * ColumnsCount + cell.Column] = cell;
		//	//CurrentUser = model.CurrentUser;
		//}

		//private void Model_ChangeStatusEvent(object sender, GameStatuses status)
		//{
		//	Statuse = model.GameStatus;
		//	CurrentUser = model.CurrentUser;
		//	IsRevenge = model.IsRevenge;
		//}

		protected override void LoseMethod(object parameter)
		{
			base.LoseMethod(parameter);
			//model.CancelGame();
			model.GamerSurrender();
		}
		protected override bool MoveCanMethod(object p)
		{
			bool ret = p is CellVM cell && model.CanMove(ConvertVMToDto(cell), ConvertVMToDto(CurrentUser));
			return ret;
		}
		protected override void MoveMethod(object p)
		{
			if (p is CellVM cell)
			{
				model.Move(ConvertVMToDto(cell), ConvertVMToDto(CurrentUser));
				//CurrentUser = model.CurrentUser;
			}
		}

		protected override void ShowStatisticMethod(object parameter)
		{
			//необходимо ерализовать в моделе
			base.ShowStatisticMethod(parameter);
		}


		protected override void StartNewGameMethod(object parameter)
		{
			//model.CreateGame(FirstGamer, SecondGamer);

			//RowsCount = (int)args[0];
			//ColumnsCount = (int)args[1];
			//LineLength = (int)args[2];
			//Gamers = ((IEnumerable<UserDto>)args[3]).OrderBy(gmr => gmr.Turn).ToArray();



			model.StartNewGame(RowsCount, ColumnsCount, LineLength, new UserDto[] { ConvertVMToDto(FirstGamer), ConvertVMToDto(SecondGamer) });
			//UpdateCells();
			base.StartNewGameMethod(parameter);
			//CurrentUser = model.CurrentUser;
		}

		protected override void RepairGameMethod(object parameter)
		{
			model.Load();
			//model.StartNewGame();
			//UpdateCells();
			//model.CreateGame(FirstGamer, SecondGamer);
			//try
			//{
			//	//for (int i = 0; i < saveGame.Cells.Count(); i++)
			//	//{
			//	//	CellContent type;
			//	//	string xmlType = saveGame.Cells[i].CellType;
			//	//	switch (xmlType)
			//	//	{
			//	//		case "Cross": type = CellContent.Cross; break;
			//	//		case "Zero": type = CellContent.Zero; break;
			//	//		case "Empty": type = CellContent.Empty; break;
			//	//		default: throw new Exception("При попытке загрузки сохраненной игры возникла ошибка");
			//	//	}
			//	//	//model.Cells = new CellDto(Cells[i].Column, Cells[i].Row, type);
			//	//	Cells[i] = new CellDto(Cells[i].Column, Cells[i].Row, type);
			//	//}

			//	//FirstGamer.UserName = model.FirstGamer.UserName;
			//	//SecondGamer.UserName = model.SecondGamer.UserName;
			//	//FirstGamer.ImageIndex = model.FirstGamer.ImageIndex;
			//	//SecondGamer.ImageIndex = model.SecondGamer.ImageIndex;

			//	FirstGamer.Image = PiecesCollection.ElementAt(FirstGamer.ImageIndex);
			//	SecondGamer.Image = PiecesCollection.ElementAt(SecondGamer.ImageIndex);
			//	/*model.FirstGamer.Image*/// = Picturies.ToArray();
			//							  //[model.FirstGamer.ImageIndex];

			//}
			//catch (Exception ex) { throw ex; }
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
