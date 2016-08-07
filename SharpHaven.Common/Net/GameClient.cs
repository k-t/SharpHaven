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
		private readonly MessageBus messageBus;

		public GameClient(GameClientConfiguration config)
		{
			this.config = config;
			this.messageBus = new MessageBus();
			this.state = GameClientState.Initial;
		}

		public IMessageSource Messages
		{
			get { return messageBus; }
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

			session = new NetworkGame(config.GameServerAddress, messageBus);
			session.Start(userName, cookie);

			this.state = GameClientState.Connected;
		}

		public void Close()
		{
			session.Stop();
			this.state = GameClientState.Initial;
		}

		public void Send<TMessage>(TMessage message)
		{
			CheckConnected();
			session.Send(message);
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
