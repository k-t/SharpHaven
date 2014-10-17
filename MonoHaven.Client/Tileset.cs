using System;
using System.Collections.Generic;
using System.Linq;
using MonoHaven.Graphics;
using MonoHaven.Resources.Layers;

namespace MonoHaven
{
	public class Tileset
	{
		private readonly WeightList<Texture> groundTiles;
		private readonly WeightList<Texture>[] borderTransitions;
		private readonly WeightList<Texture>[] crossTransitions;
		private readonly bool hasTransitions;

		public Tileset(bool hasTransitions, IEnumerable<TileData> tiles)
		{
			this.hasTransitions = hasTransitions;
			this.groundTiles = new WeightList<Texture>();
			if (hasTransitions)
			{
				this.crossTransitions = new WeightList<Texture>[15];
				this.borderTransitions = new WeightList<Texture>[15];
				for (int i = 0; i < 15; i++)
				{
					this.crossTransitions[i] = new WeightList<Texture>();
					this.borderTransitions[i] = new WeightList<Texture>();
				}
			}
			foreach (var tile in tiles)
			{
				if (tile.Type == 'g')
					groundTiles.Add(Texture.FromImageData(tile.ImageData), tile.Weight);
				if (tile.Type == 'c' && hasTransitions)
					crossTransitions[tile.Id - 1].Add(Texture.FromImageData(tile.ImageData), tile.Weight);
				if (tile.Type == 'b' && hasTransitions)
					borderTransitions[tile.Id - 1].Add(Texture.FromImageData(tile.ImageData), tile.Weight);
			}
		}

		public WeightList<Texture>[] BorderTransitions
		{
			get { return borderTransitions; }
		}

		public WeightList<Texture>[] CrossTransitions
		{
			get { return crossTransitions; }
		}

		public WeightList<Texture> GroundTiles
		{
			get { return groundTiles; }
		}

		public bool HasTransitions
		{
			get { return hasTransitions; }
		}
	}
}
