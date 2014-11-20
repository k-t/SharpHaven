using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Resources;

namespace MonoHaven.Graphics.Sprites
{
	public abstract class Sprite : ISprite
	{
		protected readonly SpriteSheet parts;

		protected Sprite(Resource res)
		{
			var neg = res.GetLayer<NegData>();
			var center = neg != null ? neg.Center : Point.Empty;
			parts = new SpriteSheet(res.GetLayers<ImageData>(), center);
			
		}

		public abstract IEnumerable<SpritePart> Parts { get; }

		public virtual void Dispose()
		{
			parts.Dispose();
		}

		public virtual void Tick(int dt)
		{
		}
	}
}
