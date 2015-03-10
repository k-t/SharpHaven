using System.Linq;
using MonoHaven.Game.Messages;
using MonoHaven.Graphics.Sprites;

namespace MonoHaven.Game
{
	public class GobUpdater : IGobDeltaVisitor
	{
		private readonly GameSession session;
		private GobChangeset changeset;
		private Gob gob;

		public GobUpdater(GameSession session)
		{
			this.session = session;
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
				delta.Visit(this);
			}
		}

		#region IGobDeltaVisitor implementation

		void IGobDeltaVisitor.Visit(GobDelta.AdjustMovement delta)
		{
		}

		void IGobDeltaVisitor.Visit(GobDelta.Avatar delta)
		{
			var sprites = delta.ResourceIds.Select(x => session.GetSprite(x));
			var sprite = new LayeredSprite(sprites);
			var delayed = new Delayed<ISprite>(sprite);
			gob.SetAvatar(delayed);
		}

		void IGobDeltaVisitor.Visit(GobDelta.Buddy delta)
		{
		}

		void IGobDeltaVisitor.Visit(GobDelta.Clear delta)
		{
			session.State.Objects.Remove(changeset.GobId, changeset.Frame);
			gob = null;
		}

		void IGobDeltaVisitor.Visit(GobDelta.DrawOffset delta)
		{
			gob.DrawOffset = delta.Value;
		}

		void IGobDeltaVisitor.Visit(GobDelta.Follow delta)
		{
		}

		void IGobDeltaVisitor.Visit(GobDelta.Health delta)
		{
		}

		void IGobDeltaVisitor.Visit(GobDelta.Homing delta)
		{
		}

		void IGobDeltaVisitor.Visit(GobDelta.Layers delta)
		{
			var sprites = delta.ResourceIds.Select(x => session.GetSprite(x));
			var sprite = new LayeredSprite(sprites);
			var delayed = new Delayed<ISprite>(sprite);
			gob.SetSprite(delayed);
		}

		void IGobDeltaVisitor.Visit(GobDelta.Light delta)
		{
		}

		void IGobDeltaVisitor.Visit(GobDelta.Overlay delta)
		{
		}

		void IGobDeltaVisitor.Visit(GobDelta.Position delta)
		{
			gob.Position = delta.Value;
		}

		void IGobDeltaVisitor.Visit(GobDelta.Resource delta)
		{
			var res = session.GetSprite(delta.Id, delta.SpriteData);
			gob.SetSprite(res);
		}

		void IGobDeltaVisitor.Visit(GobDelta.Speech delta)
		{
		}

		void IGobDeltaVisitor.Visit(GobDelta.StartMovement delta)
		{
		}

		#endregion
	}
}
