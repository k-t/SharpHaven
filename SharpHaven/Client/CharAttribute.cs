using System;

namespace SharpHaven.Client
{
	public class CharAttribute
	{
		public CharAttribute(string name, int baseValue, int modValue)
		{
			Name = name;
			BaseValue = baseValue;
			ModifiedValue = modValue;
		}

		public event Action Changed;

		public string Name { get; }

		public int BaseValue { get; private set; }

		public int ModifiedValue { get; private set; }

		public void Update(int baseValue, int modValue)
		{
			BaseValue = baseValue;
			ModifiedValue = modValue;
			Changed.Raise();
		}
	}
}
