﻿using SharpHaven.Client;

namespace SharpHaven.UI.Widgets
{
	public class BeliefBinding : AttributeBinding
	{
		private readonly bool inv;
		private readonly BeliefWidget widget;

		public BeliefBinding(CharAttribute attribute, BeliefWidget widget, bool inv)
			: base(attribute)
		{
			this.widget = widget;
			this.inv = inv;
			UpdateWidget();
		}

		protected override void OnAttributeChange()
		{
			UpdateWidget();
		}

		private void UpdateWidget()
		{
			int val = attribute.ModifiedValue;
			if (inv) val = -val;
			widget.SetSliderPosition(val);
		}
	}
}
