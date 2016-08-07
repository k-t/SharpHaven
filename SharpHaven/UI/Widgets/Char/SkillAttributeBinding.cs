using System;
using SharpHaven.Client;
using SharpHaven.Graphics;

namespace SharpHaven.UI.Widgets
{
	public class SkillAttributeBinding : AttributeBinding
	{
		private readonly Label label;
		private int baseValue;
		private int compValue;
		private int cost;

		public SkillAttributeBinding(CharAttribute attribute, Label label)
			: base(attribute)
		{
			this.label = label;
			this.baseValue = attribute.BaseValue;
			this.compValue = attribute.ModifiedValue;
			UpdateLabel();
		}

		public event Action CostChanged;

		public string AttributeName
		{
			get { return attribute.Name; }
		}

		public int BaseValue
		{
			get { return baseValue; }
		}

		public int Cost
		{
			get { return cost; }
		}

		public void Add(int value)
		{
			if (baseValue + value < attribute.BaseValue)
				return;

			baseValue += value;
			compValue += value;
			UpdateLabel();

			if (value > 0)
				cost += baseValue * 100;
			else
				cost -= (baseValue - value) * 100;

			CostChanged.Raise();
		}

		protected override void OnAttributeChange()
		{
			baseValue = attribute.BaseValue;
			compValue = attribute.ModifiedValue;
			cost = 0;
			UpdateLabel();
		}

		private void UpdateLabel()
		{
			label.Text = compValue.ToString();
			label.TextColor = GetDisplayColor();
			label.Tooltip = GetTooltip();
		}

		private Color GetDisplayColor()
		{
			if (baseValue > attribute.BaseValue)
				return Color.FromArgb(128, 128, 255);
			if (attribute.ModifiedValue > attribute.BaseValue)
				return CharWindow.BuffColor;
			if (attribute.ModifiedValue < attribute.BaseValue)
				return CharWindow.DebuffColor;
			return Color.White;
		}

		private Tooltip GetTooltip()
		{
			if (attribute.ModifiedValue < attribute.BaseValue)
				return new Tooltip(string.Format("{0} - {1}",
					attribute.BaseValue,
					attribute.BaseValue - attribute.ModifiedValue));
			if(attribute.ModifiedValue > attribute.BaseValue)
				return new Tooltip(string.Format("{0} + {1}",
					attribute.BaseValue,
					attribute.ModifiedValue - attribute.BaseValue));
			return null;
		}
	}
}
