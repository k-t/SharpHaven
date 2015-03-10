namespace MonoHaven.Network.Messages
{
	public class CharAttribute
	{
		private readonly string name;
		private int baseValue;
		private int compValue;

		public CharAttribute(string name, int baseValue, int compValue)
		{
			this.name = name;
			this.baseValue = baseValue;
			this.compValue = compValue;
		}

		public string Name
		{
			get { return name; }
		}

		public int BaseValue
		{
			get { return baseValue; }
		}

		public int ComputedValue
		{
			get { return compValue; }
		}

		public void Update(int baseValue, int compValue)
		{
			this.baseValue = baseValue;
			this.compValue = compValue;
		}
	}
}
