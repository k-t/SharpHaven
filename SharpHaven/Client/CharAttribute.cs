using System;

namespace SharpHaven.Client
{
	public class CharAttribute
	{
		private readonly string name;
		private int baseValue;
		private int modValue;

		public CharAttribute(string name, int baseValue, int modValue)
		{
			this.name = name;
			this.baseValue = baseValue;
			this.modValue = modValue;
		}

		public event Action Changed;

		public string Name
		{
			get { return name; }
		}

		public int BaseValue
		{
			get { return baseValue; }
		}

		public int ModifiedValue
		{
			get { return modValue; }
		}

		public void Update(int baseValue, int modValue)
		{
			this.baseValue = baseValue;
			this.modValue = modValue;
			Changed.Raise();
		}
	}
}
