﻿using System;
using System.Drawing;
using OpenTK.Input;
using SharpHaven.Graphics;
using SharpHaven.Input;
using SharpHaven.Utils;

namespace SharpHaven.UI.Widgets
{
	public class InventoryWidget : Widget, IItemDropTarget
	{
		private static readonly Drawable tile;

		static InventoryWidget()
		{
			tile = App.Resources.Get<Drawable>("gfx/hud/invsq");
		}

		private Point inventorySize;

		public InventoryWidget(Widget parent) : base(parent)
		{
		}

		public event Action<Point> Drop;
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
			inventorySize = new Point(rows, columns);
			int w = (tile.Width - 1) * rows - 1;
			int h = (tile.Height - 1) * columns - 1;
			Resize(w, h);
		}

		public void SetInventorySize(Point size)
		{
			SetInventorySize(size.X, size.Y);
		}

		#region IItemDropTarget

		bool IItemDropTarget.Drop(Point p, Point ul, KeyModifiers mods)
		{
			var dropPoint = MapFromScreen(ul).Add(15);
			Drop.Raise(new Point(dropPoint.X / tile.Width, dropPoint.Y / tile.Height));
			return true;
		}

		bool IItemDropTarget.Interact(Point p, Point ul, KeyModifiers mods)
		{
			return false;
		}

		#endregion
	}
}