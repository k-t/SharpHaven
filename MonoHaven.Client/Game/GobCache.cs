using System.Collections.Generic;
using System.Linq;
using C5;

namespace MonoHaven.Game
{
	public class GobCache : IEnumerable<Gob>
	{
		private readonly TreeDictionary<int, Gob> objects;
		private readonly List<Gob> localObjects;

		public GobCache()
		{
			objects = new TreeDictionary<int, Gob>();
			localObjects = new List<Gob>();
		}

		public Gob Get(int id)
		{
			Gob gob;
			if (!objects.Find(ref id, out gob))
			{
				gob = new Gob(id);
				objects[id] = gob;
			}
			return gob;
		}

		public void AddLocal(Gob gob)
		{
			localObjects.Add(gob);
		}

		public void RemoveLocal(Gob gob)
		{
			localObjects.Remove(gob);
		}

		public Gob Get(int id, int frame)
		{
			return Get(id);
		}

		public void Remove(int id, int frame)
		{
			objects.Remove(id);
		}

		public IEnumerator<Gob> GetEnumerator()
		{
			return objects.Values.Union(localObjects).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Tick(int dt)
		{
			foreach (var gob in objects.Values)
				gob.Tick(dt);
		}
	}
}
