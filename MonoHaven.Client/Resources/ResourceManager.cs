using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Resources.Layers;
using OpenTK;

namespace MonoHaven.Resources
{
	public class ResourceManager
	{
		private readonly IResourceSource defaultSource = new FolderSource("haven-res/res");
		private readonly Dictionary<string, Tileset> tilesetCache = new Dictionary<string, Tileset>();
		private readonly Dictionary<string, ISprite> spriteCache = new Dictionary<string, ISprite>();

		public Resource Get(string resName)
		{
			return defaultSource.Get(resName);
		}

		public MouseCursor GetCursor(string resName)
		{
			MouseCursor cursor;
			var cursorData = Get(resName).GetLayer<ImageData>();

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

		public Tileset GetTileset(string resName)
		{
			Tileset tileset;
			if (tilesetCache.TryGetValue(resName, out tileset))
				return tileset;

			var res = Get(resName);
			var data = res.GetLayer<TilesetData>();
			var tiles = res.GetLayers<TileData>();

			tileset = new Tileset(data, tiles);
			tilesetCache[resName] = tileset;

			return tileset;
		}

		public TextureSlice GetTexture(string resName)
		{
			var image = Get(resName).GetLayer<ImageData>();
			if (image == null)
				throw new ResourceLoadException(string.Format("Couldn't find image layer in the resource '{0}'", resName));
			// FIXME: it actually loads texture into GPU memory and this is wrong!
			return TextureSlice.FromBitmap(image.Data);
		}

		public ISprite GetSprite(string resName)
		{
			ISprite sprite;
			if (!spriteCache.TryGetValue(resName, out sprite))
			{
				var res = Get(resName);
				sprite = CreateSprite(res);
				spriteCache[resName] = sprite;
			}
			return sprite;
		}

		private ISprite CreateSprite(Resource res)
		{
			var anim = res.GetLayer<AnimData>();
			return (anim != null)
				? new AnimSprite(res)
				: new ImageSprite(res) as ISprite;
		}
	}
}
