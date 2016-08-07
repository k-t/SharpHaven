using System;
using OpenTK.Input;
using SharpHaven.Graphics;
using SharpHaven.Input;
using SharpHaven.Utils;

namespace SharpHaven.UI.Widgets
{
	public class ISBox : Widget, IItemDropTarget
	{
		private static readonly Drawable background;

		static ISBox()
		{
			background = App.Resources.Get<Drawable>("gfx/hud/bosq");
		}

		private int remaining;
		private int available;
		private int built;
		private readonly Label label;

		public ISBox(Widget parent) : base(parent)
		{
			Size = background.Size;

			label = new Label(this, Fonts.Heading);
			label.Move(40, (Height - label.Height) / 2);
			UpdateLabel();
		}

		public event Action Click;
		public event Action Transfer;
		public event Action<TransferEvent> Transfer2;
		public event Action ItemDrop;
		public event Action ItemInteract;

		public Drawable Image
		{
			get;
			set;
		}

		public int Remaining
		{
			get { return remaining; }
			set
			{
				remaining = value;
				UpdateLabel();
			}
		}

		public int Available
		{
			get { return available; }
			set
			{
				available = value;
				UpdateLabel();
			}
		}

		public int Built
		{
			get { return built; }
			set
			{
				built = value;
				UpdateLabel();
			}
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(background, 0, 0);
			if (Image != null)
				dc.Draw(Image, 6, (Height - Image.Height) / 2);
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Left)
			{
				if (e.Modifiers.HasShift())
					Transfer.Raise();
				else
					Click.Raise();
				e.Handled = true;
			}
		}

		protected override void OnMouseWheel(MouseWheelEvent e)
		{
			Transfer2.Raise(new TransferEvent(Math.Sign(-e.Delta), e.Modifiers));
		}

		private void UpdateLabel()
		{
			label.Text = $"{Remaining}/{Available}/{Built}";
		}

		#region IItemDropTarget

		bool IItemDropTarget.Drop(Coord2D p, Coord2D ul, KeyModifiers mods)
		{
			ItemDrop.Raise();
			return true;
		}

		bool IItemDropTarget.Interact(Coord2D p, Coord2D ul, KeyModifiers mods)
		{
			ItemInteract.Raise();
			return true;
		}

		#endregion
	}
}
