using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Resources;
using MonoHaven.Resources.Layers;
using MonoHaven.Utils;

namespace MonoHaven.Graphics.Sprites
{
	public class AnimSprite : Sprite
	{
		private Point center;
		private AnimFrame[] frames;
		private int frameIndex;

		public AnimSprite(Resource res)
		{
			Init(res);
		}

		public override void Dispose()
		{
			foreach (var frame in frames)
				frame.Tex.Dispose();
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			if (frames.Length > 0)
			{
				var tex = frames[frameIndex].Tex;
				tex.Draw(batch, x - center.X, y - center.Y, tex.Width, tex.Height);
			}
			Tick();
		}

		private void Tick()
		{
			frameIndex++;
			if (frameIndex >= frames.Length)
				frameIndex = 0;
		}

		private void Init(Resource res)
		{
			var anims = res.GetLayers<AnimData>();
			var parts = res.GetLayers<ImageData>();

			var neg = res.GetLayer<NegData>();
			center = neg != null ? neg.Center : Point.Empty;

			var staticParts = new List<ImageData>();
			foreach (var part in parts)
				if (part.Id < 0)
					staticParts.Add(part);

			List<ImageData>[] frames = null;
			foreach (var anim in anims)
			{
				if (frames == null)
					frames = new List<ImageData>[anim.Frames.Length];
				if (frames.Length != anim.Frames.Length)
					throw new Exception("Attempting to combine animations of different lengths");
				for (int i = 0; i < frames.Length; i++)
				{
					if (frames[i] == null)
						frames[i] = new List<ImageData>(staticParts);
					frames[i].AddRange(parts.Where(x => x.Id == anim.Frames[i]));
				}
			}
			if (frames == null)
			{
				this.frames = new AnimFrame[0];
				return;
			}
			this.frames = new AnimFrame[frames.Length];
			for (int i = 0; i < frames.Length; i++)
				using (var bitmap = ImageUtils.Combine(frames[i]))
					this.frames[i] = new AnimFrame(0, TextureRegion.FromBitmap(bitmap));
		}
	}
}
