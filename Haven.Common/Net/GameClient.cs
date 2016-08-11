using System;
using Haven.Messaging;

namespace Haven.Net
{
	public class GameClient
	{
		private readonly IAuthHandlerFactory authHandlerFactory;
		private readonly IProtocolHandlerFactory protocolHandlerFactory;
		private readonly MessageBus messageBus;
		private IProtocolHandler protocolHandler;
		private GameClientState state;
		private string userName;
		private byte[] cookie;

		public GameClient(
			IAuthHandlerFactory authHandlerFactory,
			IProtocolHandlerFactory protocolHandlerFactory)
		{
			this.authHandlerFactory = authHandlerFactory;
			this.protocolHandlerFactory = protocolHandlerFactory;
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

			using (var authHandler = authHandlerFactory.Create())
			{
				authHandler.Connect();
				if (!authHandler.TryPassword(userName, password, out cookie))
					return AuthResult.Fail();

				this.userName = userName;
				this.state = GameClientState.Authenticated;
				var token = requestToken ? authHandler.GetToken() : null;
				return AuthResult.Success(token);
			}
		}

		public AuthResult Authenticate(string userName, byte[] token)
		{
			CheckNotConnected();

			using (var authHandler = authHandlerFactory.Create())
			{
				authHandler.Connect();
				if (!authHandler.TryToken(userName, token, out cookie))
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

			protocolHandler = protocolHandlerFactory.Create(messageBus);
			protocolHandler.Connect(userName, cookie);

			this.state = GameClientState.Connected;
		}

		public void Close()
		{
			protocolHandler.Close();
			this.state = GameClientState.Initial;
		}

		public void Send<TMessage>(TMessage message)
		{
			CheckConnected();
			protocolHandler.Send(message);
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
