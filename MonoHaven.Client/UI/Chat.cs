namespace MonoHaven.UI
{
	public class Chat : Widget
	{
		private readonly Label title;

		public Chat(Widget parent, string title, bool closable) : base(parent)
		{
			this.title = new Label(this);
			this.title.AutoSize = true;
			this.title.Text = title;
		}

		public string Title
		{
			get { return title.Text; }
		}
	}
}
