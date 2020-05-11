namespace ModelLibrary
{
	//public delegate void GameOverHandler(object sender);
	//public delegate void GameOverDrawHandler(object sender);
	public delegate void NotifyChangedCellHandler(object sender, CellDto cell);
	public delegate void NotifyChangedStateHandler(object sender, ChangedStateHandlerArgs e);

	public class ChangedStateHandlerArgs
	{
		public NamesState StateName { get; }
		public object OldValue { get; }
		public object NewValue { get; }

		public ChangedStateHandlerArgs(NamesState stateName)
		{
			StateName = stateName;
		}

		public ChangedStateHandlerArgs(NamesState stateName, object newValue) : this(stateName)
		{
			NewValue = newValue;
		}

		public ChangedStateHandlerArgs(NamesState stateName, object oldValue, object newValue) : this(stateName, oldValue)
		{
			NewValue = newValue;
		}
	}
	//public delegate void ChangeStatusHandler(object sender, GameStatuses status);
}