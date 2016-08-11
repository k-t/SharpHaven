using Haven;
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
			label.Text = $"{attribute.ModifiedValue}";
			label.TextColor = GetDisplayColor();
			label.Tooltip = GetTooltip();
		}

		private Color GetDisplayColor()
		{
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
			if (attribute.ModifiedValue > attribute.BaseValue)
				return new Tooltip(string.Format("{0} + {1}",
					attribute.BaseValue,
					attribute.ModifiedValue - attribute.BaseValue));
			return null;
		}
	}
}
