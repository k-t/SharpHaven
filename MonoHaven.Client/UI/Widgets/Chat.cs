using System.Drawing;

namespace MonoHaven.UI.Widgets
{
	public class Chat : Widget
	{
		private readonly string title;
		private readonly Label label;

		public Chat(Widget parent, string title, bool closable) : base(parent)
		{
			this.title = title;
			this.label = new Label(this, Fonts.Text);
			this.label.TextColor = Color.White;
			this.label.Move(5, 5);
		}

		public string Title
		{
			get { return title; }
		}

		public void AddMessage(string text, Color? color)
		{
			label.Text = text;
		}
	}
}
