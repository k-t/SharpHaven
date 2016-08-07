using System;
using SharpHaven.Client;
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
			var data = res.GetLayer<TilesetLayer>();
			var tileset = new Tileset(data);
			foreach (var tile in res.GetLayers<TileLayer>())
			{
				var image = tileAtlas.Value.Add(tile.ImageData);
				tileset.AddTile(tile.Id, tile.Type, tile.Weight, new Picture(image, null));
			}
			return tileset;
		}
	}
}
