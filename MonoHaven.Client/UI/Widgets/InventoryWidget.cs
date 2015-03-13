using System.Drawing;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class InventoryWidget : Widget
	{
		private static readonly Drawable tile;

		static InventoryWidget()
		{
			tile = App.Resources.GetImage("gfx/hud/invsq");
		}

		private Point inventorySize;

		public InventoryWidget(Widget parent) : base(parent)
		{
		}

		protected override void OnDraw(DrawingContext dc)
		{
			for (int x = 0; x < inventorySize.X; x++)
				for (int y = 0; y < inventorySize.Y; y++)
					dc.Draw(tile, (tile.Width - 1) * x, (tile.Height - 1) * y);
		}

		public void SetInventorySize(int rows, int columns)
		{
			inventorySize = new Point(rows, columns);
			int w = (tile.Width - 1) * rows - 1;
			int h = (tile.Height - 1) * columns - 1;
			Resize(w, h);
		}

		public void SetInventorySize(Point size)
		{
			SetInventorySize(size.X, size.Y);
		}
	}
}
