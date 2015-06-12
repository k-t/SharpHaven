using System.Drawing;
using SharpHaven.Client;

namespace SharpHaven.UI.Widgets
{
	public class ExpModBinding : AttributeBinding
	{
		private readonly Label label;

		public ExpModBinding(CharAttribute attribute, Label label)
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
			label.Text = string.Format("{0}%", attribute.ComputedValue);
			label.TextColor = GetDisplayColor();
		}

		private Color GetDisplayColor()
		{
			if (attribute.ComputedValue > 100)
				return CharWindow.BuffColor;
			if (attribute.ComputedValue < 100)
				return CharWindow.DebuffColor;
			return Color.White;
		}
	}
}
