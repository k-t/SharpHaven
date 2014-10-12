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
			return GetLayers<T>().FirstOrDefault();
		}

		public IEnumerable<T> GetLayers<T>() where T : ILayer
		{
			return _layers.OfType<T>();
		}
	}
}

