#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;

namespace MonoHaven
{
	public delegate bool Promise<T>(out T value);

	public class Delayed<T>
	{
		private T value;
		private Promise<T> promise;

		public Delayed(T value)
		{
			this.value = value;
		}

		public Delayed(Promise<T> promise)
		{
			if (promise == null)
				throw new ArgumentNullException("promise");
			this.promise = promise;
			this.value = default(T);
		}

		public T Value
		{
			get
			{
				if (promise != null && promise(out value))
					// ensure that closures are not lingering
					promise = null;
				return value;
			}
		}
	}
}
