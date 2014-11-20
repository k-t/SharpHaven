using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Resources;

namespace MonoHaven.Graphics.Sprites
{
	public abstract class Sprite : ISprite
	{
		private readonly Point center;
		private readonly SpriteSheet parts;

		protected Sprite(Resource res)
		{
			parts = new SpriteSheet(res.GetLayers<ImageData>());
			var neg = res.GetLayer<NegData>();
			center = neg != null ? neg.Center : Point.Empty;
		}

		protected Point Center
		{
			get { return center; }
		}

		protected IEnumerable<SpritePart> Parts
		{
			get { return parts; }
		}

		public virtual void Dispose()
		{
			parts.Dispose();
		}

		public abstract void Draw(SpriteBatch batch, int x, int y);

		protected void DrawPart(SpritePart part, SpriteBatch batch, int x, int y)
		{
			part.Tex.Draw(
				batch,
				x - Center.X + part.Offset.X,
				y - Center.Y + part.Offset.Y,
				part.Width,
				part.Height);
		}
	}
}
