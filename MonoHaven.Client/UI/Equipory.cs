using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Utils;

namespace MonoHaven.UI
{
	public class Equipory : Window
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
			background = App.Resources.GetImage("gfx/hud/equip/bg");
		}

		private readonly GobCache gobCache;
		private int gobId;
		private readonly ItemWidget[] items;

		public Equipory(Widget parent, GobCache gobCache) : base(parent, "Equipment")
		{
			this.gobCache = gobCache;
			this.gobId = -1;
			this.items = new ItemWidget[slotPositions.Length];
			foreach (var position in slotPositions)
			{
				var inv = new InventoryWidget(this);
				inv.Move(position);
				inv.SetInventorySize(1, 1);
			}
			Pack();
		}

		public void SetItem(int i, Delayed<Drawable> item, string tooltip)
		{
			if (items[i] != null)
				items[i].Remove();

			if (item != null)
			{
				items[i] = new ItemWidget(this, item);
				items[i].Move(slotPositions[i].X, slotPositions[i].Y);
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
	}
}
