using System.Collections.Generic;

namespace MonoHaven.Resources
{
	public interface IEnumerableResourceSource : IResourceSource
	{
		/// <summary>
		/// Allows to enumerate all resources contained in the source.
		/// </summary>
		IEnumerable<string> EnumerateAll();
	}
}

