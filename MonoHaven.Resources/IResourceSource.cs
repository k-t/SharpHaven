using System;
using System.IO;

namespace MonoHaven.Resources
{
	public interface IResourceSource : IDisposable
	{
		string Name { get; }
		Resource Get(string resourceName);
	}
}

