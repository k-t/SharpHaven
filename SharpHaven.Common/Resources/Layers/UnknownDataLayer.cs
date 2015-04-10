namespace SharpHaven.Resources
{
	public class UnknownDataLayer
	{
		private readonly string type;

		public UnknownDataLayer(string type)
		{
			this.type = type;
		}

		public string Type
		{
			get { return type; }
		}
	}
}

