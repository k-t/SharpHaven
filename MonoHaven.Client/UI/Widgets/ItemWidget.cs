using System;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
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
		private readonly TextBlock numLabel;
		private bool isSizeFixed;
		private Point? dragOffset;

		public ItemWidget(Widget parent, Point? dragOffset)
			: base(parent)
		{
			numLabel = new TextBlock(Fonts.Text);
			numLabel.TextColor = Color.White;

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
		public event Action<Input.KeyModifiers> Interact;

		public Item Item
		{
			get { return item; }
			set
			{
				item = value;
				isSizeFixed = false;
				// HACK:
				base.Tooltip = null;
				UpdateLabels();
			}
		}

		public override Tooltip Tooltip
		{
			get
			{
				if (base.Tooltip == null)
					base.Tooltip = GetTooltip();
				return base.Tooltip;
			}
			set { }
		}

		protected override void OnDispose()
		{
			numLabel.Dispose();
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
				dc.Draw(numLabel, 1, 1);
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

		private void DropOn(Widget widget, Point p, Input.KeyModifiers mods)
		{
			foreach (var child in widget.GetChildrenAt(p))
			{
				if (child == this) continue;
				var dropTarget = child as IDropTarget;
				if (dropTarget != null && dropTarget.Drop(p, p.Sub(dragOffset.Value), mods))
					break;
			}
		}

		private void InteractWith(Widget widget, Point p, Input.KeyModifiers mods)
		{
			foreach (var child in widget.GetChildrenAt(p))
			{
				if (child == this) continue;
				var dropTarget = child as IDropTarget;
				if (dropTarget != null && dropTarget.ItemInteract(p, p.Sub(dragOffset.Value), mods))
					break;
			}
		}

		private Tooltip GetTooltip()
		{
			if (item == null || item.Tooltip == null)
				return null;

			var text = item.Tooltip;
			if (item.Quality > 0)
				text += ", quality " + item.Quality;

			return new Tooltip(text);
		}
		
		private void FixSize()
		{
			Resize(item.Image.Size);
			isSizeFixed = true;
		}

		private void UpdateLabels()
		{
			numLabel.Clear();
			if (item != null && item.Amount >= 0)
				numLabel.Append(item.Amount.ToString());
		}

		#region IDropTarget

		bool IDropTarget.Drop(Point p, Point ul, Input.KeyModifiers mods)
		{
			return false;
		}

		bool IDropTarget.ItemInteract(Point p, Point ul, Input.KeyModifiers mods)
		{
			Interact.Raise(mods);
			return true;
		}

		#endregion
	}
}
