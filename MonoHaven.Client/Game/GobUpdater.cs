using System.Linq;
using MonoHaven.Graphics.Sprites;
using MonoHaven.Messages;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class GobUpdater
	{
		private readonly GameSession session;
		private UpdateGobMessage message;
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

		public void ApplyChanges(UpdateGobMessage message)
		{
			this.message = message;
			this.gob = null;

			var objectCache = session.State.Objects;
			if (message.ReplaceFlag)
				objectCache.Remove(message.GobId, message.Frame - 1);
			
			foreach (var delta in message.Deltas)
			{
				if (this.gob == null)
					this.gob = objectCache.Get(message.GobId, message.Frame);
				deltaMatcher.Match(delta);
			}
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
			if (string.IsNullOrEmpty(delta.Name) && delta.Group == 0 && delta.Type == 0)
				gob.KinInfo = null;
			else
				gob.KinInfo = new KinInfo(delta.Name, delta.Group, delta.Type);
		}

		private void Apply(GobDelta.Clear delta)
		{
			session.State.Objects.Remove(message.GobId, message.Frame);
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
			gob.Health = new GobHealth(delta.Value);
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
			gob.Speech = string.IsNullOrEmpty(delta.Text)
				? null
				: new GobSpeech(delta.Text, delta.Offset);
		}

		private void Apply(GobDelta.StartMovement delta)
		{
			gob.StartMovement(delta.Origin, delta.Destination, delta.TotalSteps);
		}

		private void Apply(GobDelta.AdjustMovement delta)
		{
			gob.AdjustMovement(delta.Step);
		}
	}
}
