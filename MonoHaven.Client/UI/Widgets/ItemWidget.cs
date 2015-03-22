using System;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.Utils;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class ItemWidget : Widget
	{
		private static readonly Drawable missing;
		private static readonly Point defaultSize = new Point(30, 30);

		static ItemWidget()
		{
			missing = App.Resources.Get<Drawable>("gfx/invobjs/missing");
		}

		private Item item;
		private bool isSizeFixed;
		private Point? dragOffset;

		public ItemWidget(Widget parent, Point? dragOffset)
			: base(parent)
		{
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
		public event Action<Point> Interact;

		public Item Item
		{
			get { return item; }
			set
			{
				item = value;
				isSizeFixed = false;
			}
		}

		public override Tooltip Tooltip
		{
			get
			{
				if (base.Tooltip == null && !string.IsNullOrEmpty(item.Tooltip))
					base.Tooltip = new Tooltip(item.Tooltip);
				return base.Tooltip;
			}
			set { }
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
						if (e.Modifiers.HasFlag(Input.KeyModifiers.Shift))
							Transfer.Raise(p);
						else if (e.Modifiers.HasFlag(Input.KeyModifiers.Control))
							Drop.Raise(p);
						else
							Take.Raise(p);
						e.Handled = true;
						break;
					case MouseButton.Right:
						Interact.Raise(p);
						e.Handled = true;
						break;
				}
			}
			else
			{
				switch (e.Button)
				{
					case MouseButton.Left:
						DropOn(Parent, e.Position);
						e.Handled = true;
						break;
					case MouseButton.Right:
						InteractWith(Parent, e.Position);
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

		private void DropOn(Widget widget, Point p)
		{
			foreach (var child in widget.GetChildrenAt(p))
			{
				var dropTarget = child as IDropTarget;
				if (dropTarget != null && dropTarget.Drop(p, p.Sub(dragOffset.Value)))
					break;
			}
		}

		private void InteractWith(Widget widget, Point p)
		{
			var wp = widget.MapFromScreen(p);
			foreach (var child in GetChildrenAt(wp))
			{
				var dropTarget = child as IDropTarget;
				if (dropTarget != null && dropTarget.ItemInteract(p, p.Sub(dragOffset.Value)))
					break;
			}
		}

		private void FixSize()
		{
			Resize(item.Image.Size);
			isSizeFixed = true;
		}
	}
}
