using System;
using Haven;
using SharpHaven.Graphics;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class Image : Widget
	{
		public Image(Widget parent) : base(parent)
		{
		}

		public event Action<MouseButtonEvent> Click;

		public Drawable Drawable
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (Drawable == null)
				return;
			dc.Draw(Drawable, 0, 0);
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			Click.Raise(e);
		}
	}
}

