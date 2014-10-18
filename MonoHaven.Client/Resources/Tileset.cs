using System.Collections.Generic;
using MonoHaven.Graphics;
using MonoHaven.Resources.Layers;

namespace MonoHaven.Resources
{
	public class Tileset
	{
		private readonly TextureAtlas atlas;
		private readonly WeightList<TextureRegion> groundTiles;
		private readonly WeightList<TextureRegion>[] borderTransitions;
		private readonly WeightList<TextureRegion>[] crossTransitions;
		private readonly bool hasTransitions;

		public Tileset(bool hasTransitions, IEnumerable<TileData> tiles)
		{
			atlas = new TextureAtlas(1024, 1024);

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

			foreach (var tile in tiles)
			{
				if (tile.Type == 'g')
						groundTiles.Add(atlas.AddImage(tile.ImageData), tile.Weight);
				if (tile.Type == 'c' && hasTransitions)
					crossTransitions[tile.Id - 1].Add(atlas.AddImage(tile.ImageData), tile.Weight);
				if (tile.Type == 'b' && hasTransitions)
					borderTransitions[tile.Id - 1].Add(atlas.AddImage(tile.ImageData), tile.Weight);
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
