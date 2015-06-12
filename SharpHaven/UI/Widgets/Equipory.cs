using System;
using System.Drawing;
using OpenTK.Input;
using SharpHaven.Client;
using SharpHaven.Graphics;

namespace SharpHaven.UI.Widgets
{
	public class Equipory : Window, IItemDropTarget
	{
		private static readonly Drawable background;
		private static readonly Point avatarPosition = new Point(32, 0);
		private static readonly Point[] slotPositions = {
			new Point(0, 0),
			new Point(244, 0),
			new Point(0, 31),
			new Point(244, 31),
			new Point(0, 62),
			new Point(244, 62),
			new Point(0, 93),
			new Point(244, 93),
			new Point(0, 124),
			new Point(244, 124),
			new Point(0, 155),
			new Point(244, 155),
			new Point(0, 186),
			new Point(244, 186),
			new Point(0, 217),
			new Point(244, 217)
		};

		static Equipory()
		{
			background = App.Resources.Get<Drawable>("gfx/hud/equip/bg");
		}

		private readonly GobCache gobCache;
		private int gobId;
		private readonly ItemWidget[] items;

		public Equipory(Widget parent, GobCache gobCache) : base(parent, "Equipment")
		{
			this.gobCache = gobCache;
			this.gobId = -1;
			this.items = new ItemWidget[slotPositions.Length];
			for (int i = 0; i < slotPositions.Length; i++)
			{
				int slotIndex = i;
				var inv = new InventoryWidget(this);
				inv.Move(slotPositions[i]);
				inv.SetInventorySize(1, 1);
				inv.Drop += (p) => Drop.Raise(slotIndex);
			}
			Pack();
		}

		public event Action<int, Point> ItemTransfer;
		public event Action<int, Point> ItemTake;
		public event Action<int, Point> ItemAct;
		public event Action<int> ItemInteract;
		public event Action<int> Drop;

		public void SetItem(int i, Item item)
		{
			Action<Point> itemTakeHandler = (p) => ItemTake.Raise(i, p);
			Action<Point> itemTransferHandler = (p) => ItemTransfer.Raise(i, p);
			Action<Point> itemActHandler = (p) => ItemAct.Raise(i, p);
			Action<KeyModifiers> itemInteractHandler = (mods) => ItemInteract.Raise(i);

			if (items[i] != null)
			{
				items[i].Take -= itemTakeHandler;
				items[i].Transfer -= itemTransferHandler;
				items[i].Act -= itemActHandler;
				items[i].Interact -= itemInteractHandler;
				items[i].Remove();
				items[i].Dispose();
			}

			if (item != null)
			{
				items[i] = new ItemWidget(this, null);
				items[i].Item = item;
				items[i].Move(slotPositions[i].X, slotPositions[i].Y);

				items[i].Take += itemTakeHandler;
				items[i].Transfer += itemTransferHandler;
				items[i].Act += itemActHandler;
				items[i].Interact += itemInteractHandler;
			}
		}

		public void SetGob(int gobId)
		{
			this.gobId = gobId;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			base.OnDraw(dc);

			// offset contents
			dc.PushMatrix();
			dc.Translate(Margin, Margin);
			dc.Draw(background, avatarPosition);
			if (gobId != -1)
			{
				var gob = gobCache.Get(gobId);
				if (gob != null && gob.Avatar != null)
					dc.Draw(gob.Avatar, avatarPosition);
			}
			dc.PopMatrix();
		}

		#region IItemDropTarget

		bool IItemDropTarget.Drop(Point p, Point ul, KeyModifiers mods)
		{
			Drop.Raise(-1);
			return true;
		}

		bool IItemDropTarget.Interact(Point p, Point ul, KeyModifiers mods)
		{
			return false;
		}

		#endregion
	}
}
