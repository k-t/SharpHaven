﻿using System.Collections.Generic;
using System.Drawing;
using C5;

namespace MonoHaven.Graphics
{
	public class TextureRegion : Drawable
	{
		private readonly Texture texture;
		private RectangleF textureBounds;

		public TextureRegion(Texture texture, int x, int y, int width, int height)
		{
			this.texture = texture;
			this.Width = width;
			this.Height = height;
			this.textureBounds = RectangleF.FromLTRB(
				(float)x / texture.Width,
				(float)y / texture.Height,
				(float)(x + width) / texture.Width,
				(float)(y + height) / texture.Height);
		}

		public override Texture GetTexture()
		{
			return texture;
		}

		public override IEnumerable<Vertex> GetVertices(Rectangle region)
		{
			return new[] {
				new Vertex(region.X, region.Y, textureBounds.Left, textureBounds.Top),
				new Vertex(region.X + Width, region.Y, textureBounds.Right, textureBounds.Top),
				new Vertex(region.X + Width, region.Y + Height, textureBounds.Right, textureBounds.Bottom),
				new Vertex(region.X, region.Y + Height, textureBounds.Left, textureBounds.Bottom)
			};
		}
	}
}
