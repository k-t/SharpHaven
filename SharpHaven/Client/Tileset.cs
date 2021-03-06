﻿using Haven.Resources;
using Haven.Utils;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Sprites;

namespace SharpHaven.Client
{
	public class Tileset
	{
		private readonly WeightList<Drawable> groundTiles;
		private readonly WeightList<ISprite> flavorObjects; 
		private readonly WeightList<Drawable>[] borderTransitions;
		private readonly WeightList<Drawable>[] crossTransitions;
		private readonly int flavorDensity;
		private readonly bool hasTransitions;

		public Tileset(TilesetLayer data)
		{
			hasTransitions = data.HasTransitions;
			flavorDensity = data.FlavorDensity;
			groundTiles = new WeightList<Drawable>();
			flavorObjects = new WeightList<ISprite>();

			if (hasTransitions)
			{
				crossTransitions = new WeightList<Drawable>[15];
				borderTransitions = new WeightList<Drawable>[15];
				for (int i = 0; i < 15; i++)
				{
					crossTransitions[i] = new WeightList<Drawable>();
					borderTransitions[i] = new WeightList<Drawable>();
				}
			}

			if (data.FlavorObjects != null)
			{
				foreach (var flavor in data.FlavorObjects)
				{
					var sprite = App.Resources.GetSprite(flavor.ResName);
					flavorObjects.Add(sprite, flavor.Weight);
				}
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

		public void AddTile(int id, char type, int weight, Drawable image)
		{
			WeightList<Drawable> targetList;
			if (type == 'g')
				targetList = groundTiles;
			else if (type == 'c' && hasTransitions)
				targetList = crossTransitions[id - 1];
			else if (type == 'b' && hasTransitions)
				targetList = borderTransitions[id - 1];
			else
				return;
			targetList.Add(image, weight);
		}
	}
}
