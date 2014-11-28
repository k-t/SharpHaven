using System.Collections.Generic;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Resources;

namespace MonoHaven.Game
{
	public class Tileset
	{
		private static TextureAtlas atlas;
		private readonly WeightList<Drawable> groundTiles;
		private readonly WeightList<ISprite> flavorObjects; 
		private readonly WeightList<Drawable>[] borderTransitions;
		private readonly WeightList<Drawable>[] crossTransitions;
		private readonly int flavorDensity;

		public Tileset(TilesetData data, IEnumerable<TileData> tiles)
		{
			if (atlas == null)
				atlas = new TextureAtlas(2048, 2048);

			flavorDensity = data.FlavorDensity;
			groundTiles = new WeightList<Drawable>();
			flavorObjects = new WeightList<ISprite>();
			if (data.HasTransitions)
			{
				crossTransitions = new WeightList<Drawable>[15];
				borderTransitions = new WeightList<Drawable>[15];
				for (int i = 0; i < 15; i++)
				{
					crossTransitions[i] = new WeightList<Drawable>();
					borderTransitions[i] = new WeightList<Drawable>();
				}
			}

			foreach (var tile in tiles)
			{
				WeightList<Drawable> targetList;
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

		public WeightList<Drawable>[] BorderTransitions
		{
			get { return borderTransitions; }
		}

		public WeightList<Drawable>[] CrossTransitions
		{
			get { return crossTransitions; }
		}

		public WeightList<Drawable> GroundTiles
		{
			get { return groundTiles; }
		}

		public WeightList<ISprite> FlavorObjects
		{
			get { return flavorObjects; }
		}
	}
}
