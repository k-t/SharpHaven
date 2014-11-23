using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Graphics.Sprites;

namespace MonoHaven.Game
{
	public class GobDelta
	{
		private readonly GameSession session;
		private readonly int id;
		private readonly int frame;
		private readonly bool replace;
		private readonly List<Func<Gob, bool>> changeset;

		public GobDelta(GameSession session, int id, int frame, bool replace)
		{
			this.session = session;
			this.id = id;
			this.frame = frame;
			this.replace = replace;

			changeset = new List<Func<Gob, bool>>();
		}

		public void Apply()
		{
			var objectCache = session.State.Objects;
			if (replace)
				objectCache.Remove(id, frame - 1);
			Gob gob = null;
			foreach (var change in changeset)
			{
				if (gob == null)
					gob = objectCache.Get(id, frame);
				if (!change(gob))
				{
					objectCache.Remove(id, frame);
					gob = null;
				}
			}
		}

		public void Remove()
		{
			AddChange(o => false);
		}

		public void MoveStart(Point orig, Point dest, int time)
		{
		}

		public void MoveAdjust(int time)
		{
		}

		public void MoveStop()
		{
		}

		public void SetAvatar(IEnumerable<int> layers)
		{
			AddChange(o => {
				var sprites = layers.Select(x => session.GetSprite(x));
				var sprite = new LayeredSprite(sprites);
				var delayed = new Delayed<ISprite>(sprite);
				o.SetAvatar(delayed);
				return true;
			});
		}

		public void SetBuddy(string name, byte group, byte type)
		{
		}
		
		public void SetDrawOffset(Point offset)
		{
			AddChange(o => {
				o.DrawOffset = offset;
				return true;
			});
		}

		public void SetHealth(byte hp)
		{
		}

		public void SetLayers(int baseResId, IEnumerable<int> layers)
		{
			AddChange(o =>
			{
				var sprites = layers.Select(x => session.GetSprite(x));
				var sprite = new LayeredSprite(sprites);
				var delayed = new Delayed<ISprite>(sprite);
				o.SetSprite(delayed);
				return true;
			});
		}

		public void SetLight(Point offset, int size, byte intensity)
		{
		}

		public void SetOverlay(int overlayId, bool prs, int resId, byte[] spriteData)
		{
		}

		public void SetPosition(Point position)
		{
			AddChange(o => {
				o.Position = position;
				return true;
			});
		}

		public void SetResource(int resId, byte[] data)
		{
			AddChange(o => {
				var res = session.GetSprite(resId, data);
				o.SetSprite(res);
				return true;
			});
		}

		public void SetSpeech(Point offset, string text)
		{
		}

		public void SetFollow(int oid, int szo, Point offset)
		{
		}

		public void ResetFollow()
		{
		}

		public void SetHoming(int oid, Point target, int velocity)
		{
		}

		public void SetHoming(Point target, int velocity)
		{
		}

		public void ResetHoming()
		{
		}

		private void AddChange(Func<Gob, bool> change)
		{
			changeset.Add(change);
		}
	}
}
