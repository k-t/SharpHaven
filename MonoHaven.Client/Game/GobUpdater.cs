using System.Linq;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Network.Messages;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class GobUpdater
	{
		private readonly GameSession session;
		private GobChangeset changeset;
		private Gob gob;
		private TypeMatcher deltaMatcher;

		public GobUpdater(GameSession session)
		{
			this.session = session;
			this.deltaMatcher = new TypeMatcher()
				.Case<GobDelta.AdjustMovement>(Apply)
				.Case<GobDelta.Avatar>(Apply)
				.Case<GobDelta.Buddy>(Apply)
				.Case<GobDelta.Clear>(Apply)
				.Case<GobDelta.DrawOffset>(Apply)
				.Case<GobDelta.Follow>(Apply)
				.Case<GobDelta.Health>(Apply)
				.Case<GobDelta.Homing>(Apply)
				.Case<GobDelta.Layers>(Apply)
				.Case<GobDelta.Light>(Apply)
				.Case<GobDelta.Overlay>(Apply)
				.Case<GobDelta.Position>(Apply)
				.Case<GobDelta.Resource>(Apply)
				.Case<GobDelta.Speech>(Apply)
				.Case<GobDelta.StartMovement>(Apply);
		}

		public void ApplyChanges(GobChangeset changeset)
		{
			this.changeset = changeset;
			this.gob = null;

			var objectCache = session.State.Objects;
			if (changeset.ReplaceFlag)
				objectCache.Remove(changeset.GobId, changeset.Frame - 1);
			
			foreach (var delta in changeset.Deltas)
			{
				if (this.gob == null)
					this.gob = objectCache.Get(changeset.GobId, changeset.Frame);
				deltaMatcher.Match(delta);
			}
		}

		private void Apply(GobDelta.AdjustMovement delta)
		{
		}

		private void Apply(GobDelta.Avatar delta)
		{
			var sprites = delta.ResourceIds.Select(x => session.GetSprite(x));
			var sprite = new LayeredSprite(sprites);
			var delayed = new Delayed<ISprite>(sprite);
			gob.SetAvatar(delayed);
		}

		private void Apply(GobDelta.Buddy delta)
		{
		}

		private void Apply(GobDelta.Clear delta)
		{
			session.State.Objects.Remove(changeset.GobId, changeset.Frame);
			gob = null;
		}

		private void Apply(GobDelta.DrawOffset delta)
		{
			gob.DrawOffset = delta.Value;
		}

		private void Apply(GobDelta.Follow delta)
		{
		}

		private void Apply(GobDelta.Health delta)
		{
		}

		private void Apply(GobDelta.Homing delta)
		{
		}

		private void Apply(GobDelta.Layers delta)
		{
			var sprites = delta.ResourceIds.Select(x => session.GetSprite(x));
			var sprite = new LayeredSprite(sprites);
			var delayed = new Delayed<ISprite>(sprite);
			gob.SetSprite(delayed);
		}

		private void Apply(GobDelta.Light delta)
		{
		}

		private void Apply(GobDelta.Overlay delta)
		{
		}

		private void Apply(GobDelta.Position delta)
		{
			gob.Position = delta.Value;
		}

		private void Apply(GobDelta.Resource delta)
		{
			var res = session.GetSprite(delta.Id, delta.SpriteData);
			gob.SetSprite(res);
		}

		private void Apply(GobDelta.Speech delta)
		{
		}

		private void Apply(GobDelta.StartMovement delta)
		{
		}
	}
}
