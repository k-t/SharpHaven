using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MonoHaven.Graphics;
using MonoHaven.Resources.Layers;
using OpenTK;

namespace MonoHaven.Resources
{
	public static class ResourceManager
	{
		private static readonly IResourceSource defaultSource = new FolderSource("haven-res/res");
		private static readonly Dictionary<string, Tileset> tilesetCache = new Dictionary<string, Tileset>();

		public static Resource LoadResource(string resName)
		{
			return defaultSource.Get(resName);
		}

		public static MouseCursor LoadCursor(string resName)
		{
			MouseCursor cursor;
			var cursorData = LoadResource(resName).GetLayer<ImageData>();

			using (var ms = new MemoryStream(cursorData.Data))
			using (var bitmap = new Bitmap(ms))
			{
				var bitmapData = bitmap.LockBits(
					new Rectangle(0, 0, bitmap.Width, bitmap.Height),
					System.Drawing.Imaging.ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				cursor = new MouseCursor(1, 1, bitmap.Width, bitmap.Height, bitmapData.Scan0);
				bitmap.UnlockBits(bitmapData);
				return cursor;
			}
		}

		public static Tileset LoadTileset(string resName)
		{
			Tileset tileset;
			if (tilesetCache.TryGetValue(resName, out tileset))
				return tileset;

			var res = LoadResource(resName);
			var data = res.GetLayer<TilesetData>();
			var tiles = res.GetLayers<TileData>();

			tileset = new Tileset(data, tiles);
			tilesetCache[resName] = tileset;

			return tileset;
		}

		public static Texture LoadTexture(string resName)
		{
			var image = LoadResource(resName).GetLayer<ImageData>();
			if (image == null)
				throw new ResourceLoadException(string.Format("Couldn't find image layer in the resource '{0}'", resName));
			// TODO: it actually loads texture into GPU memory and this is wrong!
			return new Texture(image.Data);
		}
	}
}
