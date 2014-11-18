﻿using System.Collections.Generic;
using System.Linq;
using MonoHaven.Resources;
using MonoHaven.Utils;

namespace MonoHaven.Graphics
{
	public class Avatar : Drawable
	{
		private readonly Texture tex;

		public Avatar(IEnumerable<Delayed<Resource>> layers)
		{
			var images = layers.SelectMany(x => x.Value.GetLayers<ImageData>());

			using (var bitmap = ImageUtils.Combine(images))
				tex = Texture.FromBitmap(bitmap);

			Width = tex.Width;
			Height = tex.Height;
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			tex.Draw(batch, x, y, w, h);
		}

		public override void Dispose()
		{
			if (tex != null)
				tex.Dispose();
		}
	}
}
