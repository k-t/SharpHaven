namespace MonoHaven.UI.Widgets
{
	public class Hud : Widget
	{
		private readonly HudMenu menu;

		public Hud(Widget parent) : base(parent)
		{
			menu = new HudMenu(parent);
		}

		public HudMenu Menu
		{
			get { return menu; }
		}
	}
}
