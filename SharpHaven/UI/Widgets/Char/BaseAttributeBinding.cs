﻿using System.Drawing;
using SharpHaven.Client;

namespace SharpHaven.UI.Widgets
{
	public class BaseAttributeBinding : AttributeBinding
	{
		private readonly Label label;

		public BaseAttributeBinding(CharAttribute attribute, Label label)
			: base(attribute)
		{
			this.label = label;
			UpdateLabel();
		}

		protected override void OnAttributeChange()
		{
			UpdateLabel();
		}

		private void UpdateLabel()
		{
			label.Text = string.Format("{0}", attribute.ComputedValue);
			label.TextColor = GetDisplayColor();
			label.Tooltip = GetTooltip();
		}

		private Color GetDisplayColor()
		{
			if (attribute.ComputedValue > attribute.BaseValue)
				return CharWindow.BuffColor;
			if (attribute.ComputedValue < attribute.BaseValue)
				return CharWindow.DebuffColor;
			return Color.White;
		}

		private Tooltip GetTooltip()
		{
			if (attribute.ComputedValue < attribute.BaseValue)
				return new Tooltip(string.Format("{0} - {1}",
					attribute.BaseValue,
					attribute.BaseValue - attribute.ComputedValue));
			if (attribute.ComputedValue > attribute.BaseValue)
				return new Tooltip(string.Format("{0} + {1}",
					attribute.BaseValue,
					attribute.ComputedValue - attribute.BaseValue));
			return null;
		}
	}
}
