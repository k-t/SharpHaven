using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Graphics.Sprites;

namespace MonoHaven.Game
{
	public class GameObjectDelta
	{
		private readonly GameSession session;
		private readonly int id;
		private readonly int frame;
		private readonly bool replace;
		private readonly List<Func<GameObject, bool>> changeset;

		public GameObjectDelta(GameSession session, int id, int frame, bool replace)
		{
			this.session = session;
			this.id = id;
			this.frame = frame;
			this.replace = replace;

			changeset = new List<Func<GameObject, bool>>();
		}

		public void Apply()
		{
			var objectCache = session.State.Objects;
			if (replace)
				objectCache.Remove(id, frame - 1);
			GameObject gob = null;
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
				var resources = layers.Select(x => session.GetResource(x));
				o.SetSprite(new LayeredSprite(resources));
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
				var res = session.GetResource(resId);
				var sprite = App.Instance.Resources.GetSprite(res);
				o.SetSprite(sprite);
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

		private void AddChange(Func<GameObject, bool> change)
		{
			changeset.Add(change);
		}
	}
}
