using System.Drawing;
using System.IO;

namespace SharpHaven.Resources
{
	public class BitmapFactory : IObjectFactory<Bitmap>
	{
		public Bitmap Create(string resName, Resource res)
		{
			var imageData = res.GetLayer<ImageLayer>();
			if (imageData != null)
			{
				using (var ms = new MemoryStream(imageData.Data))
					return new Bitmap(ms);
			}
			return null;
		}
	}
}
