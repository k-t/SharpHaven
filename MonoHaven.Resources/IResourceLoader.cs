using System;

namespace MonoHaven.Resources
{
	public interface IResourceLoader
	{
		IResource Load(string name, int version, int priority);
	}

	public static class ResourceLoaderExt
	{
		public static IResource Load(
			this IResourceLoader loader, string name)
		{
			return loader.Load(name, -1, 0);
		}

		public static IResource Load(
			this IResourceLoader loader, string name, int version)
		{
			return loader.Load(name, version, 0);
		}

		public static Image LoadImage(
			this IResourceLoader loader, string name)
		{
			var res = loader.Load(name);
			return res.GetLayer<Image>();
		}
	}
}

