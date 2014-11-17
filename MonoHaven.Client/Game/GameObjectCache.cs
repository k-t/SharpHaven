﻿using System.Collections.Generic;
using System.Drawing;
using C5;
namespace MonoHaven.Game
{
	public class GameObjectCache : IEnumerable<GameObject>
	{
		private readonly TreeDictionary<int, GameObject> objects;

		public GameObjectCache()
		{
			objects = new TreeDictionary<int, GameObject>();
		}

		public GameObject Get(int id, int frame)
		{
			GameObject gob;
			if (!objects.Find(ref id, out gob))
			{
				gob = new GameObject();
				objects[id] = gob;
			}
			return gob;
		}

		public void Remove(int id, int frame)
		{
			objects.Remove(id);
		}

		public IEnumerator<GameObject> GetEnumerator()
		{
			return objects.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}