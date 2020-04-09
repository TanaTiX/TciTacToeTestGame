﻿using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Common
{
	public interface IStatisticVM
	{
		/// <summary>Список игроков</summary>
		ObservableCollection<User> Users { get; }

		/// <summary>Вернуться на 1ю панель</summary>
		ICommand ExitStatisticCommand { get; }

	}
}