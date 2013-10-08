using System;

namespace MonoHaven.Resources
{
	public class SimpleResourceLoader : IResourceLoader, IDisposable
	{
		private readonly ILayerFactory layerFactory;
		private readonly IResourceSource src;

		public SimpleResourceLoader(IResourceSource src)
			: this(new LayerFactory(), src)
		{}

		public SimpleResourceLoader(
			ILayerFactory layerFactory,
			IResourceSource src)
		{
			this.layerFactory = layerFactory;
			this.src = src;
		}
		
		public IResource Load(string name, int version, int priority)
		{
			var res = new Resource(name, version);
			using (var stream = src.Get(name))
			{
				if (stream != null)
				{
					res.Load(layerFactory, stream);
					return res;
				}
				else
				{
					return null;
				}
			}
		}

		public void Dispose()
		{
			if (src != null)
				src.Dispose();
		}
	}
}

