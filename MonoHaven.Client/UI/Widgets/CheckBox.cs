using System;
using OpenTK.Input;
using SharpHaven.Graphics;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class CheckBox : Widget
	{
		private static readonly Drawable box;
		private static readonly Drawable mark;

		private readonly Label label;
		private bool isChecked;

		static CheckBox()
		{
			box = App.Resources.Get<Drawable>("gfx/hud/chkbox");
			mark = App.Resources.Get<Drawable>("gfx/hud/chkmark");
		}

		public CheckBox(Widget parent) : base(parent)
		{
			label = new Label(this, Fonts.LabelText);
			label.Move(box.Width, box.Height - Fonts.LabelText.Height);
			Resize(box.Size);
		}

		public event EventHandler CheckedChanged;

		public bool IsChecked
		{
			get { return isChecked; }
			set
			{
				if (isChecked == value)
					return;
				isChecked = value;
				CheckedChanged.Raise(this, EventArgs.Empty);
			}
		}

		public string Text
		{
			get { return label.Text; }
			set { label.Text = value; }
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(box, 0, 0);
			if (IsChecked)
				dc.Draw(mark, 0, 0);
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Left)
				IsChecked = !IsChecked;
		}
	}
}
