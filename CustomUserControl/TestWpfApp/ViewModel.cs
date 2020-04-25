namespace TestWpfApp
{
	public class ViewModel : BaseINPC
	{
		private string _text;
		private string _textSecond;

		public string Text { get => _text; set => Set(ref _text, value); }
		public string TextSecond { get => _textSecond; set => Set(ref _textSecond, value); }

		public RelayCommand ChangeText { get; }
		public RelayCommand ChangeTextSecond { get; }

		public ViewModel()
		{
			ChangeText = new RelayCommand(p =>
			{
				Text = p.ToString();
			});
			ChangeTextSecond = new RelayCommand(p =>
			{
				TextSecond = p.ToString();
			});
		}
	}
}
