using System;

namespace MonoHaven.Resources
{
	public interface IResource
	{
		string Name { get; }
		int Version { get; }

		T GetLayer<T>() where T : Layer;
	}
}

