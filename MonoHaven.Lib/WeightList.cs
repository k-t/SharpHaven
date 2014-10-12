﻿using System.Collections.Generic;

namespace MonoHaven
{
	public class WeightList<T>
	{
		private readonly List<T> items = new List<T>();
		private readonly List<int> weights = new List<int>();
		private int totalWeight = 0;

		public int Count
		{
			get { return items.Count; }
		}

		public void Add(T item, int weight)
		{
			items.Add(item);
			weights.Add(weight);
			totalWeight += weight;
		}

		public T Pick(int weight)
		{
			if (items.Count == 0)
				return default(T);

			weight %= totalWeight;
			int i = 0;
			while (true)
			{
				if ((weight -= weights[i]) <= 0)
					break;
				i++;
			}
			return items[i];
		}

		public T PickRandom()
		{
			return Pick(Random.Next(totalWeight));
		}
	}
}
