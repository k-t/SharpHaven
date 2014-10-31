using System.Drawing;

namespace MonoHaven.Graphics
{
	public class TextBlockGlyph : Drawable
	{
		private readonly Glyph glyph;
		private Point position;

		public TextBlockGlyph(Glyph glyph, int gx, int gy)
		{
			this.glyph = glyph;
			this.position = new Point(gx, gy);

			if (glyph.Image != null)
			{
				Width = glyph.Image.Height;
				Height = glyph.Image.Height;
			}
		}

		public float Advance
		{
			get { return glyph.Advance; }
		}

		public Point Position
		{
			get { return position; }
			set { position = value; }
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			if (glyph.Image == null)
				return;

			glyph.Image.Draw(batch,
				x + position.X + glyph.Offset.X,
				y + position.Y + glyph.Offset.Y,
				glyph.Image.Width,
				glyph.Image.Height);
		}
	}
}
