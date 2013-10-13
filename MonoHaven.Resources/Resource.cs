using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoHaven.Resources
{
	public class Resource
	{
		private int _version;
		private List<ILayer> _layers;

		public Resource(int version, IEnumerable<ILayer> layers)
		{
			_version = version;
			_layers = new List<ILayer>(layers);
		}

		public int Version
		{
			get { return _version; }
		}
		
		public T GetLayer<T>() where T : ILayer
		{
			Type t = typeof(T);
			return (T)_layers.FirstOrDefault(l => t.IsInstanceOfType(l));
		}
	}
}

