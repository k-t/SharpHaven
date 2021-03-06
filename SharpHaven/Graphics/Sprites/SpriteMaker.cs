﻿using System;
using Haven;
using Haven.Resources;
using SharpHaven.Client;

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

		public abstract ISprite MakeInstance(Gob owner, byte[] state);

		public virtual void Dispose()
		{
			parts.Dispose();
		}
	}
}
