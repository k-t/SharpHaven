#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System.Drawing;

namespace MonoHaven.Graphics
{
	public class Glyph
	{
		public float Advance
		{
			get;
			set;
		}

		public Drawable Image
		{
			get;
			set;
		}

		public Point Offset
		{
			get;
			set;
		}
	}
}
