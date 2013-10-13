using System;

namespace MonoHaven.Resources
{
	public interface IResourceSource : IDisposable
	{
		/// <summary>
		/// Gets the source description (e.g. URL, file name).
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Returns a resource with the specified name.
		/// </summary>
		Resource Get(string resourceName);
	}
}

