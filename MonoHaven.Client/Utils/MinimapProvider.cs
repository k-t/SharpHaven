using System.Net;

namespace SharpHaven.Utils
{
	public class MinimapProvider
	{
		private readonly string baseUrl;

		public MinimapProvider(string baseUrl)
		{
			this.baseUrl = baseUrl;
		}

		public byte[] Get(string name)
		{
			using (var client = new WebClient())
			{
				client.BaseAddress = baseUrl;
				client.Headers["User-Agent"] = "Haven/1.0";
				return client.DownloadData(name + ".png");
			}
		}
	}
}
