using System;
using System.Drawing;
using MonoHaven.Resources;

namespace MonoHaven.Graphics.Sprites
{
	public abstract class SpriteFactory : IDisposable
	{
		private readonly SpriteSheet parts;

		protected SpriteFactory(Resource res)
		{
			var neg = res.GetLayer<NegData>();
			var center = neg != null ? neg.Center : Point.Empty;
			parts = new SpriteSheet(res.GetLayers<ImageData>(), center);
		}

		protected SpriteSheet Parts
		{
			get { return parts; }
		}

		public abstract ISprite Create(byte[] state);

		public virtual void Dispose()
		{
			parts.Dispose();
		}
	}
}
