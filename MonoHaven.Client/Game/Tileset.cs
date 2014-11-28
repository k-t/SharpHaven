using System.Collections.Generic;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Resources;

namespace MonoHaven.Game
{
	public class Tileset
	{
		private static TextureAtlas atlas;
		private readonly WeightList<TextureSlice> groundTiles;
		private readonly WeightList<ISprite> flavorObjects; 
		private readonly WeightList<TextureSlice>[] borderTransitions;
		private readonly WeightList<TextureSlice>[] crossTransitions;
		private readonly int flavorDensity;

		public Tileset(TilesetData data, IEnumerable<TileData> tiles)
		{
			if (atlas == null)
				atlas = new TextureAtlas(2048, 2048);

			flavorDensity = data.FlavorDensity;
			groundTiles = new WeightList<TextureSlice>();
			flavorObjects = new WeightList<ISprite>();
			if (data.HasTransitions)
			{
				crossTransitions = new WeightList<TextureSlice>[15];
				borderTransitions = new WeightList<TextureSlice>[15];
				for (int i = 0; i < 15; i++)
				{
					crossTransitions[i] = new WeightList<TextureSlice>();
					borderTransitions[i] = new WeightList<TextureSlice>();
				}
			}

			foreach (var tile in tiles)
			{
				WeightList<TextureSlice> targetList;
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
				var sprite = App.Resources.GetSprite(flavor.ResName);
				flavorObjects.Add(sprite, flavor.Weight);
			}
		}

		public int FlavorDensity
		{
			get { return flavorDensity; }
		}

		public WeightList<TextureSlice>[] BorderTransitions
		{
			get { return borderTransitions; }
		}

		public WeightList<TextureSlice>[] CrossTransitions
		{
			get { return crossTransitions; }
		}

		public WeightList<TextureSlice> GroundTiles
		{
			get { return groundTiles; }
		}

		public WeightList<ISprite> FlavorObjects
		{
			get { return flavorObjects; }
		}
	}
}
