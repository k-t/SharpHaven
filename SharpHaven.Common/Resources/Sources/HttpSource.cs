using System.IO;
using System.Net;
using SharpHaven.Resources.Serialization.Binary;

namespace SharpHaven.Resources
{
	public class HttpSource : IResourceSource
	{
		private readonly WebClient client;
		private readonly BinaryResourceSerializer serializer;

		public HttpSource(string url)
		{
			client = new WebClient();
			client.BaseAddress = url;
			client.Headers["User-Agent"] = "Haven/1.0";
			serializer = new BinaryResourceSerializer();
		}

		public string Description
		{
			get { return $"[Http] {client.BaseAddress}"; }
		}

		public Resource Get(string resourceName)
		{
			var data = client.DownloadData(resourceName + ".res");
			return serializer.Deserialize(new MemoryStream(data));
		}

		public void Dispose()
		{
			client.Dispose();
		}
	}
}
