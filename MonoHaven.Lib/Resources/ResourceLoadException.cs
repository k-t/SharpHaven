#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;

namespace MonoHaven.Resources
{
	public class ResourceLoadException : Exception
	{
		public ResourceLoadException(string message)
			: base(message)
		{
		}

		public ResourceLoadException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}

