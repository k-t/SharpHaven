using MonoHaven.Graphics;
using MonoHaven.Resources.Layers;

namespace MonoHaven.Resources
{
	public static class ResourceManager
	{
		private readonly static IResourceSource defaultSource = new FolderSource("haven-res/res");

		public static Resource LoadResource(string resName)
		{
			return defaultSource.Get(resName);
		}

		public static Tileset LoadTileset(string resName)
		{
			var res = LoadResource(resName);

			var tileset = res.GetLayer<TilesetData>();
			var tiles = res.GetLayers<TileData>();

			return new Tileset(tileset.HasTransitions, tiles);
		}

		public static Texture LoadTexture(string resName)
		{
			var image = LoadResource(resName).GetLayer<ImageData>();
			if (image == null)
				throw new ResourceLoadException(string.Format("Couldn't find image layer in the resource '{0}'", resName));
			// TODO: it actually loads texture into GPU memory and this is wrong!
			return Texture.FromImageData(image.Data);
		}
	}
}
