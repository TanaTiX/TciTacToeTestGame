using Common;
using CommonUtils;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace View
{
	public class SettingsVM : OnPropertyChangedClass, ISettingsVM
	{
		private IEnumerable<ImageSource> _piecesCollection;

		public ICommand ExitSettingsCommand { get; }

		public ICommand StartNewGameCommand { get; }


		public Gamer FirstGamer { get; } = new Gamer();

		public Gamer SecondGamer { get; } = new Gamer();

		public IEnumerable<ImageSource> PiecesCollection { get => _piecesCollection; set =>SetProperty(ref _piecesCollection , value); }

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
