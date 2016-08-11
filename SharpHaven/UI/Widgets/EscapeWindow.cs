using System;
using Haven;
using SharpHaven.Input;
using SharpHaven.UI.Layouts;

namespace SharpHaven.UI.Widgets
{
	public class EscapeWindow : Window
	{
		private readonly Button btnLogout;
		private readonly Button btnExit;

		public EscapeWindow(Widget parent) : base(parent)
		{
			btnLogout = new Button(this, 80);
			btnLogout.Text = "Logout";
			btnLogout.Click += () => Logout.Raise();

			btnExit = new Button(this, 80);
			btnExit.Text = "Exit";
			btnExit.Click += () => Exit.Raise();

			var layout = new GridLayout();
			layout.Spacing = 5;
			layout.AddWidget(btnLogout, 0, 0);
			layout.AddWidget(btnExit, 1, 0);
			layout.UpdateGeometry(0, 0, 0, 0);

			Pack();
		}

		public event Action Logout;
		public event Action Exit;

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
		}

		protected override void OnMouseButtonUp(MouseButtonEvent e)
		{
		}
	}
}
