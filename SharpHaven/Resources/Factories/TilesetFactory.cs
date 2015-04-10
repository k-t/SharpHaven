using System;
using SharpHaven.Game;
using SharpHaven.Graphics;

namespace SharpHaven.Resources
{
	public class TilesetFactory : IObjectFactory<Tileset>
	{
		private readonly Lazy<TextureAtlas> tileAtlas;

		public TilesetFactory()
		{
			tileAtlas = new Lazy<TextureAtlas>(() => new TextureAtlas(2048, 2048));
		}

		public Tileset Create(string resName, Resource res)
		{
			var data = res.GetLayer<TilesetData>();
			var tileset = new Tileset(data);
			foreach (var tile in res.GetLayers<TileData>())
			{
				var image = tileAtlas.Value.Add(tile.ImageData);
				tileset.AddTile(tile.Id, tile.Type, tile.Weight, new Picture(image, null));
			}
			return tileset;
		}
	}
}
