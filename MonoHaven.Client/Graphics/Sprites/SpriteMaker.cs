using System;
using System.Drawing;
using SharpHaven.Resources;

namespace SharpHaven.Graphics.Sprites
{
	public abstract class SpriteMaker : IDisposable
	{
		private readonly SpriteSheet parts;

		protected SpriteMaker(Resource res)
		{
			var neg = res.GetLayer<NegData>();
			var center = neg != null ? neg.Center : Point.Empty;
			parts = new SpriteSheet(res.GetLayers<ImageData>(), center);
		}

		protected SpriteSheet Parts
		{
			get { return parts; }
		}

		public abstract ISprite MakeInstance(byte[] state);

		public virtual void Dispose()
		{
			parts.Dispose();
		}
	}
}
