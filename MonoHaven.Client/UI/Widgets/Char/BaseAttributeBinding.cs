using System.Drawing;
using MonoHaven.Game;

namespace MonoHaven.UI.Widgets
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
		}

		private Color GetDisplayColor()
		{
			if (attribute.ComputedValue > attribute.BaseValue)
				return CharWindow.BuffColor;
			if (attribute.ComputedValue < attribute.BaseValue)
				return CharWindow.DebuffColor;
			return Color.White;
		}
	}
}
