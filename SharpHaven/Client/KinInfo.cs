namespace SharpHaven.Client
{
	public class KinInfo
	{
		public KinInfo(string name, byte group, byte type)
		{
			Name = name;
			Group = group;
			Type = type;
		}

		public string Name { get; }

		public byte Group { get; }

		public byte Type { get; }
	}
}
