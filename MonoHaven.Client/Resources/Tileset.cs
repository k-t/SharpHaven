using System.Collections.Generic;
using MonoHaven.Graphics;
using MonoHaven.Resources.Layers;
using MonoHaven.Utils;

namespace MonoHaven.Resources
{
	public class Tileset
	{
		private static TextureAtlas atlas;
		private readonly WeightList<TextureRegion> groundTiles;
		private readonly WeightList<TextureRegion> flavorObjects; 
		private readonly WeightList<TextureRegion>[] borderTransitions;
		private readonly WeightList<TextureRegion>[] crossTransitions;
		private readonly int flavorDensity;

		public Tileset(TilesetData data, IEnumerable<TileData> tiles)
		{
			if (atlas == null)
				atlas = new TextureAtlas(2048, 2048);

			flavorDensity = data.FlavorDensity;
			groundTiles = new WeightList<TextureRegion>();
			flavorObjects = new WeightList<TextureRegion>();
			if (data.HasTransitions)
			{
				crossTransitions = new WeightList<TextureRegion>[15];
				borderTransitions = new WeightList<TextureRegion>[15];
				for (int i = 0; i < 15; i++)
				{
					crossTransitions[i] = new WeightList<TextureRegion>();
					borderTransitions[i] = new WeightList<TextureRegion>();
				}
			}

			foreach (var tile in tiles)
			{
				WeightList<TextureRegion> targetList;
				if (tile.Type == 'g')
					targetList = groundTiles;
				else if (tile.Type == 'c' && data.HasTransitions)
					targetList = crossTransitions[tile.Id - 1];
				else if (tile.Type == 'b' && data.HasTransitions)
					targetList = borderTransitions[tile.Id - 1];
				else
					continue;

				targetList.Add(atlas.Add(tile.ImageData), tile.Weight);
			}

			foreach (var flavor in data.FlavorObjects)
			{
				var images = App.Instance.Resources.Get(flavor.ResName).GetLayers<ImageData>();
				using (var image = ImageUtils.Combine(images))
					flavorObjects.Add(atlas.Add(image), flavor.Weight);
				// TODO: else log warning
			}
		}

		public int FlavorDensity
		{
			get { return flavorDensity; }
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

		public WeightList<TextureRegion> FlavorObjects
		{
			get { return flavorObjects; }
		}
	}
}
