using System;
using MonoHaven.Graphics;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class ImageButton : Widget
	{
		private bool isPressed;

		public Texture Up { get; set; }
		public Texture Down { get; set; }

		public event EventHandler Pressed;

		protected override void OnDraw(DrawingContext g)
		{
			var tex = isPressed ? Down : Up;
			if (tex != null)
				g.Draw(tex, 0, 0);
		}

		public override void OnButtonDown(MouseButtonEventArgs e)
		{
			isPressed = true;
		}

		public override void OnButtonUp(MouseButtonEventArgs e)
		{
			isPressed = false;
			RaisePressedEvent();
		}

		private void RaisePressedEvent()
		{
			if (Pressed != null)
				Pressed(this, EventArgs.Empty);
		}
	}
}

