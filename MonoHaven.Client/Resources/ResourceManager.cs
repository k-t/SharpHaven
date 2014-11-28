using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;
using OpenTK;
using SharpFont;

namespace MonoHaven.Resources
{
	public class ResourceManager
	{
		private static readonly Library fontLibrary;

		private readonly IResourceSource defaultSource = new FolderSource("haven-res/res");
		private readonly Dictionary<string, Tileset> tilesetCache = new Dictionary<string, Tileset>();
		private readonly Dictionary<string, SpriteFactory> spriteCache = new Dictionary<string, SpriteFactory>();

		static ResourceManager()
		{
			fontLibrary = new Library();
		}

		public Resource Get(string resName)
		{
			return defaultSource.Get(resName);
		}

		public Bitmap GetBitmap(string resName)
		{
			var imageData = Get(resName).GetLayer<ImageData>();
			if (imageData == null)
				throw new ResourceException(string.Format("Couldn't find image layer in the resource '{0}'", resName));
			using (var ms = new MemoryStream(imageData.Data))
				return new Bitmap(ms);
		}

		public MouseCursor GetCursor(string resName)
		{
			using (var bitmap = GetBitmap(resName))
			{
				var bitmapData = bitmap.LockBits(
					new Rectangle(0, 0, bitmap.Width, bitmap.Height),
					System.Drawing.Imaging.ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				var cursor = new MouseCursor(1, 1, bitmap.Width, bitmap.Height, bitmapData.Scan0);
				bitmap.UnlockBits(bitmapData);
				return cursor;
			}
		}

		public Face GetFont(string resName)
		{
			var bytes = Get(resName).GetLayer<FontData>().Data;
			return fontLibrary.NewMemoryFace(bytes, 0);
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

		public Drawable GetTexture(string resName)
		{
			var res = Get(resName);

			var imageData = Get(resName).GetLayer<ImageData>();
			if (imageData == null)
				throw new ResourceException(string.Format("Couldn't find image layer in the resource '{0}'", resName));

			// load texture
			var tex = TextureSlice.FromBitmap(imageData.Data);
			// check whether image is a ninepatch
			var ninepatch = res.GetLayer<NinepatchData>();
			if (ninepatch != null)
			{
				return new NinePatch(tex, ninepatch.Left, ninepatch.Right,
					ninepatch.Top, ninepatch.Bottom);
			}
			return tex;
		}

		public ISprite GetSprite(string resName, byte[] state = null)
		{
			SpriteFactory spriteFactory;
			if (!spriteCache.TryGetValue(resName, out spriteFactory))
			{
				var res = Get(resName);
				spriteFactory = CreateSpriteFactory(res);
				spriteCache[resName] = spriteFactory;
			}
			return spriteFactory.Create(state);
		}

		private SpriteFactory CreateSpriteFactory(Resource res)
		{
			var anim = res.GetLayer<AnimData>();
			return (anim != null)
				? new AnimSpriteFactory(res)
				: new StaticSpriteFactory(res) as SpriteFactory;
		}
	}
}
