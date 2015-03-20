using System;
using System.Collections.Generic;
using MonoHaven.Graphics;
using OpenTK;
using System.Drawing;
using MonoHaven.Input;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class ListBox : Widget
	{
		private const int ItemHeight = 20;
		private static readonly Color HighlightColor = Color.FromArgb(128, 255, 255, 0);

		private readonly List<ListBoxItem> items;
		private readonly List<TextBlock> itemLabels;
		private readonly Scrollbar scrollbar;
		private int selectedIndex;

		public ListBox(Widget parent) : base(parent)
		{
			items = new List<ListBoxItem>();
			itemLabels = new List<TextBlock>();
			scrollbar = new Scrollbar(this);
			selectedIndex = -1;
		}

		public event Action SelectedIndexChanged;

		private int DisplayItemCount
		{
			get { return Height / ItemHeight; }
		}

		public int SelectedIndex
		{
			get { return selectedIndex; }
			set
			{
				var newIndex = MathHelper.Clamp(value, -1, items.Count - 1);
				if (newIndex == selectedIndex)
					return;
				selectedIndex = newIndex;
				SelectedIndexChanged.Raise();
			}
		}

		public void Add(ListBoxItem item)
		{
			items.Add(item);
			Update();
		}

		public void AddRange(IEnumerable<ListBoxItem> collection)
		{
			items.AddRange(collection);
			Update();
		}

		public void Clear()
		{
			items.Clear();
			Update();
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.SetColor(Color.Black);
			dc.DrawRectangle(0, 0, Width, Height);
			dc.ResetColor();

			for (int i = 0; i < DisplayItemCount; i++)
			{
				int scrollIndex = scrollbar.Value + i;
				if (scrollIndex >= items.Count)
					break;

				int y = i * ItemHeight;

				if (scrollIndex == SelectedIndex)
				{
					dc.SetColor(HighlightColor);
					dc.DrawRectangle(0, y, Width, ItemHeight);
					dc.ResetColor();
				}

				var item = items[scrollIndex];
				dc.Draw(item.Image, 0, y, ItemHeight, ItemHeight);
				
				var label = itemLabels[scrollIndex];
				dc.Draw(label, ItemHeight + 2, y + (ItemHeight - label.Font.Height) / 2);
			}
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Left)
			{
				var p = MapFromScreen(e.Position);
				int index = (p.Y / ItemHeight) + scrollbar.Value;
				if (index >= items.Count)
					index = -1;
				SelectedIndex = index;
				e.Handled = true;
			}
		}

		protected override void OnMouseWheel(MouseWheelEvent e)
		{
			scrollbar.Value -= e.Delta;
		}

		protected override void OnSizeChanged()
		{
			scrollbar.Move(Width - scrollbar.Width, 0);
			scrollbar.Resize(scrollbar.Width, Height);
		}

		private void Update()
		{
			SelectedIndex = MathHelper.Clamp(SelectedIndex, -1, items.Count - 1);
			UpdateLabels();
			UpdateScrollbar();
		}

		private void UpdateLabels()
		{
			// dispose old labels
			foreach (var label in itemLabels)
				label.Dispose();
			itemLabels.Clear();

			foreach (var item in items)
			{
				var label = new TextBlock(Fonts.LabelText);
				label.TextColor = item.TextColor;
				label.Append(item.Text);
				itemLabels.Add(label);
			}
		}

		private void UpdateScrollbar()
		{
			scrollbar.Value = 0;
			scrollbar.Max = items.Count - DisplayItemCount;
		}
	}
}
