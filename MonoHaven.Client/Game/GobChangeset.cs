using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Game.Messages;
using MonoHaven.Graphics.Sprites;

namespace MonoHaven.Game
{
	public class GobChangeset : IGobDeltaVisitor
	{
		private readonly GameSession session;
		private readonly int id;
		private readonly int frame;
		private readonly bool replace;
		private readonly List<Func<Gob, bool>> changeset;

		public GobChangeset(GameSession session, int id, int frame, bool replace)
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

		private void AddChange(Func<Gob, bool> change)
		{
			changeset.Add(change);
		}

		#region IGobDeltaVisitor implementation

		public void Visit(GobDelta.AdjustMovement delta)
		{
		}

		public void Visit(GobDelta.Avatar delta)
		{
			AddChange(o =>
			{
				var sprites = delta.ResourceIds.Select(x => session.GetSprite(x));
				var sprite = new LayeredSprite(sprites);
				var delayed = new Delayed<ISprite>(sprite);
				o.SetAvatar(delayed);
				return true;
			});
		}

		public void Visit(GobDelta.Buddy delta)
		{
		}

		public void Visit(GobDelta.Clear delta)
		{
			AddChange(o => false);
		}

		public void Visit(GobDelta.DrawOffset delta)
		{
			AddChange(o =>
			{
				o.DrawOffset = delta.Value;
				return true;
			});
		}

		public void Visit(GobDelta.Follow delta)
		{
		}

		public void Visit(GobDelta.Health delta)
		{
		}

		public void Visit(GobDelta.Homing delta)
		{
		}

		public void Visit(GobDelta.Layers delta)
		{
			AddChange(o =>
			{
				var sprites = delta.ResourceIds.Select(x => session.GetSprite(x));
				var sprite = new LayeredSprite(sprites);
				var delayed = new Delayed<ISprite>(sprite);
				o.SetSprite(delayed);
				return true;
			});
		}

		public void Visit(GobDelta.Light delta)
		{
		}

		public void Visit(GobDelta.Overlay delta)
		{
		}

		public void Visit(GobDelta.Position delta)
		{
			AddChange(o =>
			{
				o.Position = delta.Value;
				return true;
			});
		}

		public void Visit(GobDelta.Resource delta)
		{
			AddChange(o =>
			{
				var res = session.GetSprite(delta.Id, delta.SpriteData);
				o.SetSprite(res);
				return true;
			});
		}

		public void Visit(GobDelta.Speech delta)
		{
		}

		public void Visit(GobDelta.StartMovement delta)
		{
		}

		#endregion
	}
}
