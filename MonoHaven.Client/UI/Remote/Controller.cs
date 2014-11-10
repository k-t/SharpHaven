namespace MonoHaven.UI.Remote
{
	public abstract class Controller
	{
		private readonly int id;

		protected Controller(int id)
		{
			this.id = id;
		}

		public abstract Widget Widget { get; }

		public virtual void HandleMessage(string message, object[] args)
		{
		}
	}
}
