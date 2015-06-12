using System;

namespace SharpHaven.Game
{
	public interface IGame
	{
		event Action Stopped;

		void Start();
		void Stop();
		void AddListener(IGameEventListener listener);
		void RemoveListener(IGameEventListener listener);
		void RequestMap(int x, int y);
		void MessageWidget(ushort widgetId, string name, object[] args);
	}
}
