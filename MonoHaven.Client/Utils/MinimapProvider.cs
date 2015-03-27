using System.IO;
using System.Net;

namespace MonoHaven.Utils
{
	public class MinimapProvider
	{
		private readonly string baseUrl;

		public MinimapProvider(string baseUrl)
		{
			this.baseUrl = baseUrl;
		}

		public Stream Get(string name)
		{
			using (var client = new WebClient())
			{
				client.BaseAddress = baseUrl;
				client.Headers["User-Agent"] = "Haven/1.0";
				byte[] data = client.DownloadData(name + ".png");
				return new MemoryStream(data);
			}
		}
	}
}
