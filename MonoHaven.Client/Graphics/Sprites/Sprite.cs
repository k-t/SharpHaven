using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Resources;

namespace MonoHaven.Graphics.Sprites
{
	public abstract class Sprite : ISprite
	{
		private readonly TextureAtlas atlas;
		private readonly Point center;
		private readonly List<SpritePart> parts;

		protected Sprite(Resource res)
		{
			atlas = new TextureAtlas(1024, 1024);
			parts = new List<SpritePart>();

			var imageLayers = res.GetLayers<ImageData>().ToList();
			foreach (var image in imageLayers)
			{
				var tex = atlas.Add(image.Data);
				var sz = new Size(tex.Width, tex.Height);
				parts.Add(new SpritePart(image.Id, tex, image.DrawOffset, sz, image.Z, image.SubZ));
			}

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
			atlas.Dispose();
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
