using System.Collections.Generic;

namespace SharpHaven.Resources
{
	public class ResourceObjectCache
	{
		private readonly Dictionary<string, object> dict;

		public ResourceObjectCache()
		{
			dict = new Dictionary<string, object>();
		}

		public object Get(string resName)
		{
			object obj;
			return dict.TryGetValue(resName, out obj) ? obj : null;
		}

		public void Add(string resName, object obj)
		{
			dict[resName] = obj;
		}
	}
}
