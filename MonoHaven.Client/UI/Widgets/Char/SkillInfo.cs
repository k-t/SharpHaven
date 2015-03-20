using System.Drawing;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class SkillInfo : Widget
	{
		public SkillInfo(Widget parent) : base(parent)
		{
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.SetColor(Color.Black);
			dc.DrawRectangle(0, 0, Width, Height);
			dc.ResetColor();
		}
	}
}
