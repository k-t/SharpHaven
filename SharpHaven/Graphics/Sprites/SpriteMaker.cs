using System;
using Haven;
using Haven.Resources;

namespace SharpHaven.Graphics.Sprites
{
	public abstract class SpriteMaker : IDisposable
	{
		private readonly SpriteSheet parts;

		protected SpriteMaker(Resource res)
		{
			var neg = res.GetLayer<NegLayer>();
			var center = neg != null ? neg.Center : Point2D.Empty;
			parts = new SpriteSheet(res.GetLayers<ImageLayer>(), center);
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
