#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public class Image : Widget
	{
		public Image(Widget parent) : base(parent)
		{
		}

		public Drawable Drawable
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (Drawable == null)
				return;

			dc.Draw(Drawable, 0, 0);
		}
	}
}

