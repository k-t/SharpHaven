using System;
using System.Drawing;
using OpenTK.Input;

namespace MonoHaven.Widgets
{
	public class ImageButton : Widget
	{
		private Tex up;
		private Tex down;
		private bool isPressed;

		public ImageButton(Point p, Tex up, Tex down)
			: base(p, up.Size)
		{
			this.up = up;
			this.down = down;
		}

		public override void Draw(GOut g)
		{
			var tex = isPressed ? down : up;
			g.Draw(tex, 0, 0);
		}

		public override void OnButtonDown(MouseButtonEventArgs e)
		{
			isPressed = true;
		}

		public override void OnButtonUp(MouseButtonEventArgs e)
		{
			isPressed = false;
		}
	}
}

