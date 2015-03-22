using System;
using System.Drawing;
using MonoHaven.Resources;

namespace MonoHaven.Graphics.Sprites
{
	public abstract class SpritePrototype : IDisposable
	{
		private readonly SpriteSheet parts;

		protected SpritePrototype(Resource res)
		{
			var neg = res.GetLayer<NegData>();
			var center = neg != null ? neg.Center : Point.Empty;
			parts = new SpriteSheet(res.GetLayers<ImageData>(), center);
		}

		protected SpriteSheet Parts
		{
			get { return parts; }
		}

		public abstract ISprite CreateInstance(byte[] state);

		public virtual void Dispose()
		{
			parts.Dispose();
		}
	}
}
