#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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

