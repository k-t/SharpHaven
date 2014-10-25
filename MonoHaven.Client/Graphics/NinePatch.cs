﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MonoHaven.Graphics
{
	public class NinePatch : Drawable
	{
		private const int TopLeft = 0;
		private const int TopCenter = 1;
		private const int TopRight = 2;
		private const int MiddleLeft = 3;
		private const int MiddleCenter = 4;
		private const int MiddleRight = 5;
		private const int BottomLeft = 6;
		private const int BottomCenter = 7;
		private const int BottomRight = 8;

		private readonly Texture texture;
		private readonly Rectangle patchBounds;
		private readonly TextureRegion[] patches = new TextureRegion[9];
		
		public NinePatch(Texture texture, int left, int right, int top, int bottom)
		{
			this.texture = texture;
			this.patchBounds = Rectangle.FromLTRB(left, top, right, bottom);
			
			var textureBounds = new Rectangle(0, 0, texture.Width, texture.Height);

			this.patches = SplitToPatches(textureBounds, patchBounds)
				.Select(x => new TextureRegion(texture, x))
				.ToArray();
		}

		public override Texture GetTexture()
		{
			return texture;
		}

		public override IEnumerable<Vertex> GetVertices(Rectangle region)
		{
			var vertices = new List<Vertex>();
			var regionPatches = SplitToPatches(region, patchBounds);
			for (int i = 0; i < regionPatches.Length; i++)
			{
				if (regionPatches[i] == Rectangle.Empty)
					continue;
				vertices.AddRange(patches[i].GetVertices(regionPatches[i]));
			}
			return vertices;
		}

		private static Rectangle[] SplitToPatches(Rectangle rect, Rectangle patchBounds)
		{
			var patches = new Rectangle[9];

			int left = patchBounds.Left;
			int right = patchBounds.Right;
			int top = patchBounds.Top;
			int bottom = patchBounds.Bottom;

			int middleWidth = rect.Width - left - right;
			int middleHeight = rect.Height - top - bottom;

			if (top > 0)
			{
				if (left > 0) patches[TopLeft] = new Rectangle(0, 0, left, top);
				if (middleWidth > 0) patches[TopCenter] = new Rectangle(left, 0, middleWidth, top);
				if (right > 0) patches[TopRight] = new Rectangle(left + middleWidth, 0, right, top);
			}
			if (middleHeight > 0)
			{
				if (left > 0) patches[MiddleLeft] = new Rectangle(0, top, left, middleHeight);
				if (middleWidth > 0) patches[MiddleCenter] = new Rectangle(left, top, middleWidth, middleHeight);
				if (right > 0) patches[MiddleRight] = new Rectangle(left + middleWidth, top, right, middleHeight);
			}
			if (bottom > 0)
			{
				if (left > 0) patches[BottomLeft] = new Rectangle(0, top + middleHeight, left, bottom);
				if (middleWidth > 0) patches[BottomCenter] = new Rectangle(left, top + middleHeight, middleWidth, bottom);
				if (right > 0) patches[BottomRight] = new Rectangle(left + middleWidth, top + middleHeight, right, bottom);
			}

			// If split only vertical, move splits from right to center.
			if (left == 0 && middleWidth == 0)
			{
				patches[TopCenter] = patches[TopRight];
				patches[MiddleCenter] = patches[MiddleRight];
				patches[BottomCenter] = patches[BottomRight];
				patches[TopRight] = Rectangle.Empty;
				patches[MiddleRight] = Rectangle.Empty;
				patches[BottomRight] = Rectangle.Empty;
			}
			// If split only horizontal, move splits from bottom to center.
			if (top == 0 && middleHeight == 0)
			{
				patches[MiddleLeft] = patches[BottomLeft];
				patches[MiddleCenter] = patches[BottomCenter];
				patches[MiddleRight] = patches[BottomRight];
				patches[BottomLeft] = Rectangle.Empty;
				patches[BottomCenter] = Rectangle.Empty;
				patches[BottomRight] = Rectangle.Empty;
			}

			// offset all rectangles
			for (int i = 0; i < patches.Length; i++)
				if (patches[i] != Rectangle.Empty)
					patches[i].Offset(rect.X, rect.Y);

			return patches;
		}
	}
}