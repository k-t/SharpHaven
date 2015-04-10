using System.Collections.Generic;

namespace SharpHaven.Resources
{
	public interface IEnumerableResourceSource : IResourceSource
	{
		/// <summary>
		/// Allows to enumerate all resources contained in the source.
		/// </summary>
		IEnumerable<string> EnumerateAll();
	}
}

