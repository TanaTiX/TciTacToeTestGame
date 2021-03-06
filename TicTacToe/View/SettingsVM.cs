﻿using Common;
using CommonUtils;
using LibVM;
using ModelLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace View
{
	public class SettingsVM : OnPropertyChangedClass, ISettingsVM
	{
		private IEnumerable<ImageSource> _piecesCollection;

		//public ICommand ExitSettingsCommand { get; }

		public ICommand StartNewGameCommand { get; }


		public UserVM FirstGamer { get; } = new UserVM();

		public UserVM SecondGamer { get; } = new UserVM();

		public IEnumerable<ImageSource> PiecesCollection { get => _piecesCollection; set =>SetProperty(ref _piecesCollection , value); }

		public ICommand ShowFirstScreenCommand { get; }

		public SettingsVM()
		{
			StartNewGameCommand = new RelayCommand
			(
				p =>
				{
					Utils.Log("смена окна после настроек");
				},
				p =>
				{
					if (string.IsNullOrWhiteSpace(FirstGamer.UserName) || string.IsNullOrWhiteSpace(SecondGamer.UserName))
						return false;
					if (FirstGamer.UserName == SecondGamer.UserName || FirstGamer.Image == SecondGamer.Image)
						return false;

					return true;
				}
			);

			IEnumerable wewqe = PiecesCollection;

		}
	}
}
