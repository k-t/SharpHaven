using MonoHaven.Game;

namespace MonoHaven.UI.Remote
{
	public abstract class Controller
	{
		private readonly int id;
		private readonly GameSession session;

		protected Controller(int id, GameSession session)
		{
			this.id = id;
			this.session = session;
		}

		public abstract Widget Widget { get; }

		protected GameSession Session
		{
			get { return session; }
		}

		public virtual void HandleMessage(string message, object[] args)
		{
		}
	}
}
