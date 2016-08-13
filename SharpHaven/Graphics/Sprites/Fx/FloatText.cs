using System;
using System.Collections.Generic;
using System.Linq;
using Haven;
using SharpHaven.Client;
using SharpHaven.Graphics.Text;

namespace SharpHaven.Graphics.Sprites.Fx
{
	public class FloatText : ISprite
	{
		private readonly TextLine text;
		private readonly SpritePart part;
		private readonly int startY;
		private double a;
		private bool disposed;

		public FloatText(string text, SpriteFont font, Color color)
		{
			this.text = new TextLine(font);
			this.text.TextColor = color;
			this.text.OutlineColor = color.Contrast();
			this.text.Append(text);
			this.startY = -(font.Height + 30);
			this.part = new SpritePart(-1, this.text, new Point2D(-this.text.TextWidth / 2, startY), 5, 0);
		}

		public int Y
		{
			get { return part.Offset.Y; }
			set { part.Offset = new Point2D(part.Offset.X, value); }
		}

		public int Height
		{
			get { return text.Height; }
		}

		public IEnumerable<SpritePart> Parts
		{
			get { yield return part; }
		}

		public bool Tick(int dt)
		{
			return disposed;
		}

		private void Dispose()
		{
			text.Dispose();
			disposed = true;
		}

		public class Layouter : ISprite
		{
			private static readonly IEnumerable<SpritePart> None = Enumerable.Empty<SpritePart>();

			private readonly Gob owner;

			public Layouter(Gob owner)
			{
				this.owner = owner;
			}

			public IEnumerable<SpritePart> Parts
			{
				get { return None; }
			}

			public bool Tick(int dt)
			{
				var floatTexts = owner.Overlays
					.Select(x => x.Sprite.Value as FloatText)
					.Where(x => x != null)
					.Reverse();

				int y = 0;
				foreach (var floatText in floatTexts)
				{
					floatText.a += dt / 2000.0D;
					floatText.Y = Math.Min(floatText.startY - (int)(10.0D * floatText.a), y);
					if (floatText.a >= 1.0D)
						floatText.Dispose();
					y = floatText.Y - floatText.Height;
				}
				return false;
			}
		}
	}
}
