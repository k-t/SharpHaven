namespace MonoHaven.Game.Messages
{
	public interface IGobDeltaVisitor
	{
		void Visit(GobDelta.AdjustMovement delta);
		void Visit(GobDelta.Avatar delta);
		void Visit(GobDelta.Buddy delta);
		void Visit(GobDelta.Clear delta);
		void Visit(GobDelta.DrawOffset delta);
		void Visit(GobDelta.Follow delta);
		void Visit(GobDelta.Health delta);
		void Visit(GobDelta.Homing delta);
		void Visit(GobDelta.Layers delta);
		void Visit(GobDelta.Light delta);
		void Visit(GobDelta.Overlay delta);
		void Visit(GobDelta.Position delta);
		void Visit(GobDelta.Resource delta);
		void Visit(GobDelta.Speech delta);
		void Visit(GobDelta.StartMovement delta);
	}
}
