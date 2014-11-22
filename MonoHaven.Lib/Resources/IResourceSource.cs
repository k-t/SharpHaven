#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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

