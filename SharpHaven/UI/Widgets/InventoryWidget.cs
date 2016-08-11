using System;
using Haven;
using OpenTK.Input;
using SharpHaven.Graphics;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class InventoryWidget : Widget, IItemDropTarget
	{
		private static readonly Drawable tile;

		static InventoryWidget()
		{
			tile = App.Resources.Get<Drawable>("gfx/hud/invsq");
		}

		private Point2D inventorySize;

		public InventoryWidget(Widget parent) : base(parent)
		{
		}

		public event Action<Point2D> Drop;
		public event Action<TransferEvent> Transfer;

		protected override void OnDraw(DrawingContext dc)
		{
			for (int x = 0; x < inventorySize.X; x++)
				for (int y = 0; y < inventorySize.Y; y++)
					dc.Draw(tile, (tile.Width - 1) * x, (tile.Height - 1) * y);
		}

		protected override void OnMouseWheel(MouseWheelEvent e)
		{
			Transfer.Raise(new TransferEvent(Math.Sign(-e.Delta), e.Modifiers));
		}

		public void SetInventorySize(int rows, int columns)
		{
			inventorySize = new Point2D(rows, columns);
			int w = (tile.Width - 1) * rows - 1;
			int h = (tile.Height - 1) * columns - 1;
			this.Resize(w, h);
		}

		public void SetInventorySize(Point2D size)
		{
			SetInventorySize(size.X, size.Y);
		}

		#region IItemDropTarget

		bool IItemDropTarget.Drop(Point2D p, Point2D ul, KeyModifiers mods)
		{
			var dropPoint = MapFromScreen(ul).Add(15);
			Drop.Raise(new Point2D(dropPoint.X / tile.Width, dropPoint.Y / tile.Height));
			return true;
		}

		bool IItemDropTarget.Interact(Point2D p, Point2D ul, KeyModifiers mods)
		{
			return false;
		}

		#endregion
	}
}
