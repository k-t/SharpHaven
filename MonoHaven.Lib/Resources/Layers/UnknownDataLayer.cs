namespace MonoHaven.Resources
{
	public class UnknownDataLayer : IDataLayer
	{
		public UnknownDataLayer(string type)
		{
			Type = type;
		}

		public string Type { get; private set; }
	}
}

