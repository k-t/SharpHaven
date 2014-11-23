using System.Drawing;
using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public class MenuGrid : Widget
	{
		private const int RowCount = 3;
		private const int ColumnCount = 3;

		private static readonly Drawable cellBackground;

		static MenuGrid()
		{
			cellBackground = App.Instance.Resources.GetTexture("gfx/hud/invsq");
		}

		public MenuGrid(Widget parent) : base(parent)
		{
			base.SetSize(
				(cellBackground.Width - 1) * ColumnCount,
				(cellBackground.Height - 1) * RowCount);
		}

		protected override void OnDraw(DrawingContext dc)
		{
			for (int i = 0; i < RowCount; i++)
				for (int j = 0; j < ColumnCount; j++)
				{
					int x = (cellBackground.Width - 1) * j;
					int y = (cellBackground.Height - 1) * i;
					dc.Draw(cellBackground, x, y);
				}
		}
	}
}
