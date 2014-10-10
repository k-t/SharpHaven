using MonoHaven.Graphics;
using MonoHaven.Resources;

namespace MonoHaven
{
	public static class ResourceManager
	{
		private readonly static IResourceSource defaultSource = new JarSource("haven-res.jar");

		public static Texture LoadTexture(string resName)
		{
			var imageData = defaultSource.Get(resName).GetLayer<ImageLayer>();
			if (imageData == null)
				throw new ResourceLoadException(string.Format("Couldn't find image layer in the resource '{0}'", resName));
			// TODO: it actually loads texture into GPU memory and this is wrong!
			return Texture.FromImage(imageData);
		}
	}
}
