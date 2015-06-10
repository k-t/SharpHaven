using SharpHaven.Game;
using System;
using SharpHaven.Game.Events;

namespace SharpHaven.Net
{
	public class NetGame : IGame
	{
		private readonly Connection connection;
		private readonly NetGameSettings settings;

		public NetGame(NetGameSettings settings)
		{
			this.settings = settings;
			this.connection = new Connection(settings.Host, settings.Port);
		}

		public event Action Stopped;

		public void Start()
		{
			connection.Open(settings.UserName, settings.Cookie);
		}

		public void Stop()
		{
			connection.Close();
		}

		public void AddListener(IGameEventListener listener)
		{
			connection.Listener.Add(listener);
		}

		public void RemoveListener(IGameEventListener listener)
		{
			connection.Listener.Remove(listener);
		}

		public void RequestMap(int x, int y)
		{
			connection.RequestMap(x, y);
		}

		public void MessageWidget(WidgetMessage message)
		{
			connection.MessageWidget(message);
		}
	}
}
