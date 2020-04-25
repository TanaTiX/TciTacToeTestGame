namespace TestWpfApp
{
	public class ViewModel : BaseINPC
	{
		private string _text;

		public string Text { get => _text; set => Set(ref _text, value); }
	}
}
