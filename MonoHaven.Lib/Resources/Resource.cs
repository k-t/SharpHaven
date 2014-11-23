using System.Collections.Generic;
using System.Linq;

namespace MonoHaven.Resources
{
	public class Resource
	{
		private int _version;
		private List<IDataLayer> _layers;

		public Resource(int version, IEnumerable<IDataLayer> layers)
		{
			_version = version;
			_layers = new List<IDataLayer>(layers);
		}

		public int Version
		{
			get { return _version; }
		}
		
		public T GetLayer<T>() where T : IDataLayer
		{
			return GetLayers<T>().FirstOrDefault();
		}

		public IEnumerable<T> GetLayers<T>() where T : IDataLayer
		{
			return _layers.OfType<T>();
		}
	}
}

