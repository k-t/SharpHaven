using System;
using System.IO;

namespace MonoHaven.Resources
{
	public interface IResourceSource : IDisposable
	{
		string Name { get; }
		Stream Get(string resourceName);
	}
}

