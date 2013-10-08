using System;
using System.Collections.Generic;

namespace MonoHaven.Resources
{
	public class LayerFactory : ILayerFactory
	{
		private readonly Dictionary<string, Func<byte[], Layer>> _constructors;

		public LayerFactory()
		{
			_constructors = new Dictionary<string, Func<byte[], Layer>>();
			RegisterDefaultLayers();
		}
		
		public Layer Make(string typeName, byte[] data)
		{
			var cons = FindConstructor(typeName);
			if (cons == null)
				throw new UnknownLayerException(typeName);
			return cons(data);
		}

		public void Register(string typeName, Func<byte[], Layer> cons)
		{
			_constructors.Add(typeName, cons);
		}

		private Func<byte[], Layer> FindConstructor(string typeName)
		{
			Func<byte[], Layer> cons;
			return _constructors.TryGetValue(typeName, out cons) ? cons : null;
		}

		private void RegisterDefaultLayers()
		{
			Register("image", Image.Make);
		}
	}
}

