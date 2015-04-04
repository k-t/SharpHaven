using System;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Text;
using MonoHaven.Input;
using MonoHaven.Utils;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class ItemWidget : Widget, IDropTarget
	{
		private static readonly Drawable missing;
		private static readonly Point defaultSize = new Point(30, 30);

		static ItemWidget()
		{
			missing = App.Resources.Get<Drawable>("gfx/invobjs/missing");
		}

		private Item item;
		private readonly TextLine lblAmount;
		private bool isSizeFixed;
		private Point? dragOffset;
		private Tooltip tooltip;

		public ItemWidget(Widget parent, Point? dragOffset)
			: base(parent)
		{
			lblAmount = new TextLine(Fonts.Text);
			lblAmount.TextColor = Color.White;

			this.dragOffset = dragOffset;
			if (dragOffset.HasValue)
			{
				Host.GrabMouse(this);
				Move(Host.MousePosition.Sub(dragOffset.Value));
			}
		}

		public event Action<Point> Transfer;
		public event Action<Point> Drop;
		public event Action<Point> Take;
		public event Action<Point> Act;
		public event Action<KeyModifiers> Interact;

		public Item Item
		{
			get { return item; }
			set
			{
				if (item != null)
					item.Changed -= OnItemChanged;

				item = value;
				OnItemChanged();

				if (item != null)
					item.Changed += OnItemChanged;
			}
		}

		public override Tooltip Tooltip
		{
			get
			{
				// HACK:
				if (tooltip == null)
					UpdateTooltip();
				return tooltip;
			}
			set { }
		}

		protected override void OnDispose()
		{
			lblAmount.Dispose();

			if (dragOffset.HasValue)
				Host.ReleaseMouse();
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (item == null || item.Image == null)
				dc.Draw(missing, 0, 0, defaultSize.X, defaultSize.Y);
			else
			{
				if (!isSizeFixed)
					FixSize();
				dc.Draw(item.Image, 0, 0);
				dc.Draw(lblAmount, 1, 1);

				if (item.Meter > 0)
				{
					double a = (item.Meter) / 100.0;
					var r = (byte)((1 - a) * 255);
					var g = (byte)(a * 255);
					var b = (byte)0;
					dc.SetColor(r, g, b, 255);
					dc.DrawRectangle(Width - 5, (int)((1 - a) * Height), 5, (int)(a * Height));
					dc.ResetColor();
				}
			}
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			var p = MapFromScreen(e.Position);
			if (!dragOffset.HasValue)
			{
				switch (e.Button)
				{
					case MouseButton.Left:
						if (e.Modifiers.HasShift())
							Transfer.Raise(p);
						else if (e.Modifiers.HasControl())
							Drop.Raise(p);
						else
							Take.Raise(p);
						e.Handled = true;
						break;
					case MouseButton.Right:
						Act.Raise(p);
						e.Handled = true;
						break;
				}
			}
			else
			{
				switch (e.Button)
				{
					case MouseButton.Left:
						DropOn(Parent, e.Position, e.Modifiers);
						e.Handled = true;
						break;
					case MouseButton.Right:
						InteractWith(Parent, e.Position, e.Modifiers);
						e.Handled = true;
						break;
				}
			}
		}

		protected override void OnMouseMove(MouseMoveEvent e)
		{
			if (dragOffset.HasValue)
				Move(e.Position.Sub(dragOffset.Value));
		}

		private void DropOn(Widget widget, Point p, KeyModifiers mods)
		{
			foreach (var child in widget.GetChildrenAt(p))
			{
				if (child == this) continue;
				var dropTarget = child as IDropTarget;
				if (dropTarget != null && dropTarget.Drop(p, p.Sub(dragOffset.Value), mods))
					break;
			}
		}

		private void InteractWith(Widget widget, Point p, KeyModifiers mods)
		{
			foreach (var child in widget.GetChildrenAt(p))
			{
				if (child == this) continue;
				var dropTarget = child as IDropTarget;
				if (dropTarget != null && dropTarget.ItemInteract(p, p.Sub(dragOffset.Value), mods))
					break;
			}
		}

		private void OnItemChanged()
		{
			isSizeFixed = false;
			UpdateTooltip();
			UpdateAmountLabel();
		}
		
		private void FixSize()
		{
			Resize(item.Image.Size);
			isSizeFixed = true;
		}

		private void UpdateTooltip()
		{
			if (item != null && !string.IsNullOrEmpty(item.Tooltip))
			{
				var text = item.Tooltip;
				if (item.Quality > 0)
					text += ", quality " + item.Quality;
				tooltip = new Tooltip(text);
			}
			else
				tooltip = null;
		}

		private void UpdateAmountLabel()
		{
			lblAmount.Clear();
			if (item != null && item.Amount >= 0)
				lblAmount.Append(item.Amount.ToString());
		}

		#region IDropTarget

		bool IDropTarget.Drop(Point p, Point ul, KeyModifiers mods)
		{
			return false;
		}

		bool IDropTarget.ItemInteract(Point p, Point ul, KeyModifiers mods)
		{
			Interact.Raise(mods);
			return true;
		}

		#endregion
	}
}
