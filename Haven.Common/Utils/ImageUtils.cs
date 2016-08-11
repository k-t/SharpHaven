using System.Drawing;
using System.IO;

namespace Haven.Utils
{
	public static class ImageUtils
	{
		public static string GetImageFileExtension(byte[] imageData)
		{
			using (var image = Image.FromStream(new MemoryStream(imageData)))
			{
				var formatName = new ImageFormatConverter().ConvertToString(image.RawFormat);
				return !string.IsNullOrEmpty(formatName)
					? "." + formatName.ToLower()
					: null;
			}
		}
	}
}
