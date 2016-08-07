using System.Collections.Specialized;

namespace SharpHaven.Resources.Serialization.Ini
{
	public class IniLayer
	{
		private readonly NameValueCollection files = new NameValueCollection();

		public object Data { get; set; }

		public NameValueCollection Files
		{
			get { return files; }
		}
	}
}
