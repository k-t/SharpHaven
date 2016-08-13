using Haven.Resources;
using SharpFont;

namespace SharpHaven.Resources
{
	public class FontFaceFactory : IObjectFactory<Face>
	{
		private static readonly Library FontLibrary = new Library();

		public Face Create(string resName, Resource res)
		{
			var bytes = res.GetLayer<FontLayer>().Bytes;
			return FontLibrary.NewMemoryFace(bytes, 0);
		}
	}
}
