using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MonoHaven.Graphics;
using MonoHaven.Resources.Layers;

namespace MonoHaven.Resources
{
	public class Tileset
	{
		private readonly Texture atlas;
		private readonly WeightList<TextureRegion> groundTiles;
		private readonly WeightList<TextureRegion>[] borderTransitions;
		private readonly WeightList<TextureRegion>[] crossTransitions;
		private readonly bool hasTransitions;

		public Tileset(bool hasTransitions, IEnumerable<TileData> tiles)
		{
			atlas = new Texture(1024, 1024);

			this.hasTransitions = hasTransitions;
			this.groundTiles = new WeightList<TextureRegion>();
			if (hasTransitions)
			{
				this.crossTransitions = new WeightList<TextureRegion>[15];
				this.borderTransitions = new WeightList<TextureRegion>[15];
				for (int i = 0; i < 15; i++)
				{
					this.crossTransitions[i] = new WeightList<TextureRegion>();
					this.borderTransitions[i] = new WeightList<TextureRegion>();
				}
			}

			int x = 0;
			int y = 0;
			foreach (var tile in tiles)
			{
				using (var bitmap = new Bitmap(new MemoryStream(tile.ImageData)))
				{
					// pack tiles to atlas
					if (x + bitmap.Width > 1024)
					{
						x = 0;
						y += bitmap.Height + 2;
					}
					atlas.Upload(x, y, bitmap.Width, bitmap.Height, bitmap);
					var region = new TextureRegion(atlas, x, y, bitmap.Width, bitmap.Height);
					x += bitmap.Width + 2;

					if (tile.Type == 'g')
						groundTiles.Add(region, tile.Weight);
					if (tile.Type == 'c' && hasTransitions)
						crossTransitions[tile.Id - 1].Add(region, tile.Weight);
					if (tile.Type == 'b' && hasTransitions)
						borderTransitions[tile.Id - 1].Add(region, tile.Weight);
				}
			}
		}

		public WeightList<TextureRegion>[] BorderTransitions
		{
			get { return borderTransitions; }
		}

		public WeightList<TextureRegion>[] CrossTransitions
		{
			get { return crossTransitions; }
		}

		public WeightList<TextureRegion> GroundTiles
		{
			get { return groundTiles; }
		}

		public bool HasTransitions
		{
			get { return hasTransitions; }
		}
	}
}
