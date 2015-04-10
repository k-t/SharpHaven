namespace SharpHaven.Game
{
	public class KinInfo
	{
		private readonly string name;
		private readonly byte group;
		private readonly byte type;

		public KinInfo(string name, byte group, byte type)
		{
			this.name = name;
			this.group = group;
			this.type = type;
		}

		public string Name
		{
			get { return name; }
		}

		public byte Group
		{
			get { return group; }
		}

		public byte Type
		{
			get { return type; }
		}
	}
}
