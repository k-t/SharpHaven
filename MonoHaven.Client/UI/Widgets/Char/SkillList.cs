using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Game;

namespace MonoHaven.UI.Widgets
{
	public class SkillList : Widget
	{
		private readonly ListBox listBox;
		private readonly List<Skill> skills;
		private int maxCost;

		public SkillList(Widget parent) : base(parent)
		{
			listBox = new ListBox(this);
			listBox.SelectedIndexChanged += () => SelectedItemChanged.Raise();
			skills = new List<Skill>();
			MaxCost = int.MaxValue;
		}

		public event Action SelectedItemChanged;

		public Skill SelectedItem
		{
			get
			{
				return listBox.SelectedIndex != -1
					? skills[listBox.SelectedIndex]
					: null;
			}
			set
			{
				int index = skills.IndexOf(value);
				listBox.SelectedIndex = index;
			}
		}

		public int MaxCost
		{
			get { return maxCost; }
			set
			{
				maxCost = value;
				UpdateListBox();
			}
		}

		public void SetSkills(IEnumerable<Skill> collection)
		{
			skills.Clear();
			skills.AddRange(collection);	
			UpdateListBox();
		}

		protected override void OnSizeChanged()
		{
			listBox.Resize(Width, Height);
		}

		private void UpdateListBox()
		{
			listBox.ClearItems();
			listBox.AddItemRange(skills.Select(x => new ListBoxItem(x.Image, x.Tooltip, GetSkillLabelColor(x))));
		}

		private Color GetSkillLabelColor(Skill item)
		{
			return item.Cost > MaxCost
				? Color.FromArgb(255, 128, 128)
				: Color.White;
		}
	}
}
