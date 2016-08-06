using System;
using SharpHaven.Game;

namespace SharpHaven.Net
{
	public class GameClient
	{
		private readonly GameClientConfiguration config;
		private GameClientState state;
		private string userName;
		private byte[] cookie;
		private NetworkGame session;
		private readonly CompositeGameEventListener compositeListener;

		public GameClient(GameClientConfiguration config)
		{
			this.config = config;
			this.compositeListener = new CompositeGameEventListener();
			this.state = GameClientState.Initial;
		}

		public IGameEventSource Events
		{
			get { return compositeListener; }
		}

		public GameClientState State
		{
			get { return state; }
		}

		public AuthResult Authenticate(string userName, string password, bool requestToken)
		{
			CheckNotConnected();

			using (var authClient = new AuthClient(config.AuthServerAddress))
			{
				authClient.Connect();
				authClient.BindUser(userName);
				if (!authClient.TryPassword(password, out cookie))
					return AuthResult.Fail();

				this.userName = userName;
				this.state = GameClientState.Authenticated;
				var token = requestToken ? authClient.GetToken() : null;
				return AuthResult.Success(token);
			}
		}

		public AuthResult Authenticate(string userName, byte[] token)
		{
			CheckNotConnected();

			using (var authClient = new AuthClient(config.AuthServerAddress))
			{
				authClient.Connect();
				authClient.BindUser(userName);
				if (!authClient.TryToken(token, out cookie))
					return AuthResult.Fail();

				this.userName = userName;
				this.state = GameClientState.Authenticated;
				return AuthResult.Success();
			}
		}

		public void Connect()
		{
			CheckAuthenticated();
			CheckNotConnected();

			session = new NetworkGame(config.GameServerAddress);
			session.AddListener(compositeListener);
			session.Start(userName, cookie);

			this.state = GameClientState.Connected;
		}

		public void Close()
		{
			session.Stop();
			session.RemoveListener(compositeListener);

			this.state = GameClientState.Initial;
		}

		public void RequestMap(int x, int y)
		{
			CheckConnected();
			session.RequestMap(x, y);
		}

		public void MessageWidget(ushort widgetId, string name, object[] args)
		{
			CheckConnected();
			session.MessageWidget(widgetId, name, args);
		}

		private void CheckAuthenticated()
		{
			if (state != GameClientState.Authenticated)
				throw new InvalidOperationException("Client is not authenticated");
		}

		private void CheckConnected()
		{
			if (state != GameClientState.Connected)
				throw new InvalidOperationException("Client is not connected");
		}

		private void CheckNotConnected()
		{
			if (state == GameClientState.Connected)
				throw new InvalidOperationException("Client is currently connected");
		}
	}
}
