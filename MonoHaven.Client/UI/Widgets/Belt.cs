using System;
using System.Collections.Generic;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.UI.Layouts;
using MonoHaven.Utils;

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

		public event Action<BeltClickEventArgs> Click;

		public void SetSlot(int slot, Delayed<Drawable> image)
		{
			slots[slot].Image = image;
		}

		private void OnSlotClick(object sender, MouseButtonEvent e)
		{
			int index = slots.IndexOf((BeltSlot)sender);
			if (index != -1)
				Click.Raise(new BeltClickEventArgs(index, e.Button, e.Modifiers));
		}

		private class BeltSlot : Widget
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
		}
	}
}
