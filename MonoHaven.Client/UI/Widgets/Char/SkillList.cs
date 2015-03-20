using System;
using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Input;
using OpenTK;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class SkillList : Widget
	{
		private readonly Scrollbar scrollbar;
		private readonly List<Skill> items;
		private readonly List<TextBlock> itemLabels;
		private int selectedIndex;
		private int maxCost;

		public SkillList(Widget parent) : base(parent)
		{
			scrollbar = new Scrollbar(this);
			items = new List<Skill>();
			itemLabels = new List<TextBlock>();

			SelectedIndex = -1;
			MaxCost = int.MaxValue;
		}

		public event Action SelectedIndexChanged;

		public ICollection<Skill> Items
		{
			get { return items; }
		}

		public int SelectedIndex
		{
			get { return selectedIndex; }
			set
			{
				selectedIndex = MathHelper.Clamp(value, -1, items.Count - 1);
				SelectedIndexChanged.Raise();
			}
		}

		public Skill SelectedItem
		{
			get { return selectedIndex != -1 ? items[selectedIndex] : null; }
		}

		public int MaxCost
		{
			get { return maxCost; }
			set
			{
				maxCost = value;
				UpdateLabelColors();
			}
		}

		private int DisplayItemCount
		{
			get { return Height / 20; }
		}

		public void Bind(IEnumerable<Skill> skills)
		{
			items.Clear();
			items.AddRange(skills);
			SelectedIndex = -1;
			
			UpdateLabels();
			UpdateScrollbar();
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

				if (scrollIndex == SelectedIndex)
				{
					dc.SetColor(255, 255, 0, 128);
					dc.DrawRectangle(0, i * 20, Width, 20);
					dc.ResetColor();
				}
				
				var skill = items[scrollIndex];
				var label = itemLabels[scrollIndex];

				if (skill.Cost > MaxCost)
					dc.SetColor(255, 128, 128, 255);
				dc.Draw(skill.Image, 0, i * 20, 20, 20);
				dc.Draw(label, 25, i * 20 + (20 - label.Font.Height) / 2);
				dc.ResetColor();
			}
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Left)
			{
				var p = MapFromScreen(e.Position);
				int index = (p.Y / 20) + scrollbar.Value;
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

		private void UpdateLabels()
		{
			// dispose old labels
			foreach (var label in itemLabels)
				label.Dispose();
			itemLabels.Clear();

			foreach (var item in items)
			{
				var label = new TextBlock(Fonts.LabelText);
				label.TextColor = GetSkillLabelColor(item);
				label.Append(item.Tooltip);
				itemLabels.Add(label);
			}
		}

		private void UpdateLabelColors()
		{
			for (int i = 0; i < items.Count; i++)
				itemLabels[i].TextColor = GetSkillLabelColor(items[i]);
		}

		private void UpdateScrollbar()
		{
			scrollbar.Value = 0;
			scrollbar.Max = items.Count - DisplayItemCount;
		}

		private Color GetSkillLabelColor(Skill item)
		{
			return item.Cost > MaxCost
				? Color.FromArgb(255, 128, 128)
				: Color.White;
		}
	}
}
