using System.Drawing;

namespace MonoHaven.Graphics
{
	public class TextBlock : Drawable
	{
		private readonly SpriteFont font;

		public TextBlock(SpriteFont font)
		{
			this.font = font;
		}

		public Color Color
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			if (string.IsNullOrEmpty(Value))
				return;

			int i = x;
			int j = y + font.Ascent;

			batch.SetColor(Color);
			foreach (var c in Value)
			{
				var glyph = font.GetGlyph(c);
				if (glyph.Image != null)
					glyph.Image.Draw(batch, i + glyph.Offset.X, j + glyph.Offset.Y, glyph.Image.Width, glyph.Image.Height);
				i += (int)glyph.Advance;
			}
			batch.SetColor(Color.White);
		}
	}
}
