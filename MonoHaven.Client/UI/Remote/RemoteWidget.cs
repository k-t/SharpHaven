namespace MonoHaven.UI.Remote
{
	public abstract class RemoteWidget
	{
		private readonly int id;

		protected RemoteWidget(int id)
		{
			this.id = id;
		}

		public abstract Widget Widget { get; }

		public virtual void HandleMessage(string message, object[] args)
		{
		}
	}
}
