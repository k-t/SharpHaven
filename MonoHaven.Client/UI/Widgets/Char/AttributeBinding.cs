using System;
using MonoHaven.Game;

namespace MonoHaven.UI.Widgets
{
	public class AttributeBinding : IDisposable
	{
		protected readonly CharAttribute attribute;

		public AttributeBinding(CharAttribute attribute)
		{
			this.attribute = attribute;
			this.attribute.Changed += OnAttributeChange;
		}

		protected virtual void OnAttributeChange()
		{
		}

		public void Dispose()
		{
			attribute.Changed -= OnAttributeChange;
		}
	}
}
