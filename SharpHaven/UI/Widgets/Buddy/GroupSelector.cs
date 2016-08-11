using System;
using Haven;
using SharpHaven.Graphics;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class GroupSelector : Widget
	{
		private const int SelectorSize = 20;

		public GroupSelector(Widget parent) : base(parent)
		{
			this.Resize(BuddyGroup.Colors.Length * SelectorSize, SelectorSize);
		}

		public event Action<int> Select;

		public int? Group
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			for (int i = 0; i < BuddyGroup.Colors.Length; i++)
			{
				int x = i * SelectorSize;
				if (i == Group)
				{
					dc.ResetColor();
					dc.DrawRectangle(x, 0, SelectorSize - 1, SelectorSize - 1);
				}
				dc.SetColor(BuddyGroup.Colors[i]);
				dc.DrawRectangle(2 + x, 2, SelectorSize - 5, SelectorSize - 5);
			}
			dc.ResetColor();
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			var p = MapFromScreen(e.Position);
			if (p.Y >= 2 && p.Y <= 17)
			{
				int g = (p.X - 2) / SelectorSize;
				if (g >= 0 && g < BuddyGroup.Colors.Length &&
					p.X >= 2 + (g * SelectorSize) &&
					p.X < 17 + (g * SelectorSize))
				{
					Select.Raise(g);
					e.Handled = true;
				}
			}
		}
	}
}
