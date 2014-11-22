#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;

namespace MonoHaven.Graphics
{
	public abstract class Drawable : IDisposable
	{
		public int Width
		{
			get;
			set;
		}

		public int Height
		{
			get;
			set;
		}

		public virtual void Dispose()
		{
		}

		public abstract void Draw(SpriteBatch batch, int x, int y, int w, int h);
	}
}
