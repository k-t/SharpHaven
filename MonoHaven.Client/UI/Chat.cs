namespace MonoHaven.UI
{
	public class Chat : Widget
	{
		private readonly string title;

		public Chat(Widget parent, string title, bool closable) : base(parent)
		{
			this.title = title;
		}

		public string Title
		{
			get { return title; }
		}
	}
}
