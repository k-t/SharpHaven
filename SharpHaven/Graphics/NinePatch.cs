using System.Linq;
using Haven;

namespace SharpHaven.Graphics
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

		private readonly Rect patchBounds;
		private readonly TextureSlice[] patches;
		
		public NinePatch(TextureSlice tex, int left, int right, int top, int bottom)
		{
			size = new Point2D(tex.Width, tex.Height);
			patchBounds = Rect.FromLTRB(left, top, right, bottom);
			var textureBounds = new Rect(0, 0, tex.Width, tex.Height);
			patches = SplitToPatches(textureBounds, patchBounds)
				.Select(x => tex.Slice(x))
				.ToArray();
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			var regionPatches = SplitToPatches(new Rect(x, y, w, h), patchBounds);
			for (int i = 0; i < regionPatches.Length; i++)
			{
				if (regionPatches[i] == Rect.Empty)
					continue;
				var patch = regionPatches[i];
				patches[i].Draw(batch, patch.X, patch.Y, patch.Width, patch.Height);
			}
		}

		private static Rect[] SplitToPatches(Rect rect, Rect patchBounds)
		{
			var patches = new Rect[9];

			int left = patchBounds.Left;
			int right = patchBounds.Right;
			int top = patchBounds.Top;
			int bottom = patchBounds.Bottom;

			int middleWidth = rect.Width - left - right;
			int middleHeight = rect.Height - top - bottom;

			if (top > 0)
			{
				if (left > 0) patches[TopLeft] = new Rect(0, 0, left, top);
				if (middleWidth > 0) patches[TopCenter] = new Rect(left, 0, middleWidth, top);
				if (right > 0) patches[TopRight] = new Rect(left + middleWidth, 0, right, top);
			}
			if (middleHeight > 0)
			{
				if (left > 0) patches[MiddleLeft] = new Rect(0, top, left, middleHeight);
				if (middleWidth > 0) patches[MiddleCenter] = new Rect(left, top, middleWidth, middleHeight);
				if (right > 0) patches[MiddleRight] = new Rect(left + middleWidth, top, right, middleHeight);
			}
			if (bottom > 0)
			{
				if (left > 0) patches[BottomLeft] = new Rect(0, top + middleHeight, left, bottom);
				if (middleWidth > 0) patches[BottomCenter] = new Rect(left, top + middleHeight, middleWidth, bottom);
				if (right > 0) patches[BottomRight] = new Rect(left + middleWidth, top + middleHeight, right, bottom);
			}

			// If split only vertical, move splits from right to center.
			if (left == 0 && middleWidth == 0)
			{
				patches[TopCenter] = patches[TopRight];
				patches[MiddleCenter] = patches[MiddleRight];
				patches[BottomCenter] = patches[BottomRight];
				patches[TopRight] = Rect.Empty;
				patches[MiddleRight] = Rect.Empty;
				patches[BottomRight] = Rect.Empty;
			}
			// If split only horizontal, move splits from bottom to center.
			if (top == 0 && middleHeight == 0)
			{
				patches[MiddleLeft] = patches[BottomLeft];
				patches[MiddleCenter] = patches[BottomCenter];
				patches[MiddleRight] = patches[BottomRight];
				patches[BottomLeft] = Rect.Empty;
				patches[BottomCenter] = Rect.Empty;
				patches[BottomRight] = Rect.Empty;
			}

			// offset all rectangles
			for (int i = 0; i < patches.Length; i++)
				if (patches[i] != Rect.Empty)
					patches[i].Offset(rect.X, rect.Y);

			return patches;
		}
	}
}
