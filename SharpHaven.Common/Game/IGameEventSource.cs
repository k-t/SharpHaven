namespace SharpHaven.Game
{
	public interface IGameEventSource
	{
		void AddListener(IGameEventListener listener);
		void RemoveListener(IGameEventListener listener);
	}
}
