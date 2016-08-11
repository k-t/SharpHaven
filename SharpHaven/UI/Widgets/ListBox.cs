using System;
using System.Collections.Generic;
using Haven;
using OpenTK;
using OpenTK.Input;
using SharpHaven.Graphics;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class ListBox : Widget
	{
		private const int ItemHeight = 20;
		private static readonly Color HighlightColor = Color.FromArgb(128, 255, 255, 0);

		private readonly List<ListBoxItem> items;
		private readonly Scrollbar scrollbar;
		private int selectedIndex;

		public ListBox(Widget parent) : base(parent)
		{
			items = new List<ListBoxItem>();
			scrollbar = new Scrollbar(this);
			selectedIndex = -1;
		}

		public event Action SelectedIndexChanged;

		public ICollection<ListBoxItem> Items
		{
			get { return items; }
		}

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

		public ListBoxItem SelectedItem
		{
			get { return selectedIndex != -1 ? items[selectedIndex] : null; }
			set
			{
				if (value != null)
				{
					int index = items.IndexOf(value);
					if (index != -1)
						selectedIndex = index;
				}
				else
					selectedIndex = -1;
			}
		}

		public void AddItem(ListBoxItem item)
		{
			items.Add(item);
			Update();
		}

		public void AddItemRange(IEnumerable<ListBoxItem> collection)
		{
			items.AddRange(collection);
			Update();
		}

		public void RemoveItem(ListBoxItem item)
		{
			if (items.Remove(item))
				Update();
		}

		public void ClearItems()
		{
			foreach (var item in items)
				item.Dispose();
			items.Clear();
			Update();
		}

		public void Sort(Comparison<ListBoxItem> comparison)
		{
			var selectedItem = SelectedItem;
			items.Sort(comparison);
			SelectedItem = selectedItem;
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

				items[scrollIndex].Draw(dc, 0, y, ItemHeight);
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

		protected override void OnDispose()
		{
			foreach (var item in items)
				item.Dispose();
		}

		private void Update()
		{
			SelectedIndex = MathHelper.Clamp(SelectedIndex, -1, items.Count - 1);

			scrollbar.Value = 0;
			scrollbar.Max = items.Count - DisplayItemCount;
		}
	}
}
