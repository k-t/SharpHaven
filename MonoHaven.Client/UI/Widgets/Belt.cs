using System;
using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.UI.Layouts;
using MonoHaven.Utils;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class Belt : Widget
	{
		private const int SlotCount = 10;

		private readonly List<BeltSlot> slots;

		public Belt(Widget parent) : base(parent)
		{
			slots = new List<BeltSlot>(SlotCount);

			int w = 0;
			int h = 0;

			var layout = new GridLayout();
			for (int i = 0; i < SlotCount; i++)
			{
				var slot = new BeltSlot(this);
				slot.Label = (i + 1).ToString();
				slot.Click += OnSlotClick;
				slot.Drop += OnSlotDrop;
				slot.ItemDrop += OnSlotItemDrop;
				slots.Add(slot);

				int column = layout.ColumnCount;
				layout.AddWidget(slot, 0, column);
				layout.SetColumnWidth(column, slot.Width - 1);

				w += slot.Width - 1;
				h = Math.Max(h, slot.Height);
			}
			layout.Spacing = -1;
			layout.UpdateGeometry(0, 0, 0, 0);

			Resize(w, h);
		}

		public event Action<BeltClickEvent> Click;
		public event Action<int, GameAction> Set;
		public event Action<int> SetItem;

		public void SetSlot(int slot, Delayed<Drawable> image)
		{
			slots[slot].Image = image;
		}

		private void OnSlotClick(object sender, MouseButtonEvent e)
		{
			int index = slots.IndexOf((BeltSlot)sender);
			if (index != -1)
				Click.Raise(new BeltClickEvent(index, e.Button, e.Modifiers));
		}

		private void OnSlotDrop(object sender, DropEvent e)
		{
			var action = e.Data as GameAction;
			if (action != null)
			{
				int index = slots.IndexOf((BeltSlot)sender);
				if (index != -1)
					Set.Raise(index, action);
			}
		}

		private void OnSlotItemDrop(object sender, EventArgs e)
		{
			int index = slots.IndexOf((BeltSlot)sender);
			if (index != -1)
				SetItem.Raise(index);
		}

		private class BeltSlot : Widget, IItemDropTarget
		{
			private static readonly Drawable background;

			private readonly Label label;
			private Delayed<Drawable> image = new Delayed<Drawable>();

			static BeltSlot()
			{
				background = App.Resources.Get<Drawable>("gfx/hud/invsq");
			}

			public BeltSlot(Widget parent) : base(parent)
			{
				Resize(background.Size);

				label = new Label(this, Fonts.LabelText);
				label.Move(4, 0);
			}

			public event EventHandler<MouseButtonEvent> Click;
			public event EventHandler<DropEvent> Drop;
			public event EventHandler ItemDrop;

			public Delayed<Drawable> Image
			{
				get { return image; }
				set { image = value ?? new Delayed<Drawable>(); }
			}

			public string Label
			{
				get { return label.Text; }
				set { label.Text = value; }
			}

			protected override void OnDraw(DrawingContext dc)
			{
				dc.Draw(background, 0, 0);
				if (image.Value != null)
					dc.Draw(image.Value, 1, 1, Width - 2, Height - 2);
			}

			protected override void OnMouseButtonDown(MouseButtonEvent e)
			{
				Click.Raise(this, e);
			}

			protected override void OnDrop(DropEvent e)
			{
				Drop.Raise(this, e);
			}

			#region IItemDropTarget

			bool IItemDropTarget.Drop(Point p, Point ul, KeyModifiers mods)
			{
				ItemDrop.Raise(this, EventArgs.Empty);
				return true;
			}

			bool IItemDropTarget.Interact(Point p, Point ul, KeyModifiers mods)
			{
				return false;
			}

			#endregion
		}
	}
}
