﻿using System;

namespace MonoHaven.Network
{
	public class Connection : IDisposable
	{
		private const int PROTOCOL_VERSION = 2;

		private const int MSG_SESS = 0;
		private const int MSG_CLOSE = 8;

		private readonly ConnectionSettings settings;
		private readonly GameSocket socket;
		private readonly MessageReceiver receiver;
		private readonly MessageSender sender;
		private int state;

		public Connection(ConnectionSettings settings)
		{
			this.settings = settings;

			state = ConnectionState.Created;
			socket = new GameSocket(settings.Host, settings.Port);
			sender = new MessageSender(socket);
			receiver = new MessageReceiver(socket);
		}

		public void Dispose()
		{
			Close();
		}

		public void Open()
		{
			Connect();
			receiver.Run();
			sender.Run();
			state = ConnectionState.Opened;
		}

		public void Close()
		{
			receiver.Stop();
			sender.Stop();
			socket.Close();
			state = ConnectionState.Closed;
		}

		private void Connect()
		{
			socket.Connect();

			var hello = new Message(MSG_SESS)
				.Uint16(1)
				.String("Haven")
				.Uint16(PROTOCOL_VERSION)
				.String(settings.UserName)
				.Bytes(settings.Cookie);

			socket.SendMessage(hello);

			ConnectionError error;
			while (true)
			{
				var reply = socket.ReceiveMessage();
				if (reply.MessageType == MSG_SESS)
				{
					error = (ConnectionError)reply.ReadByte();
					break;
				}
				if (reply.MessageType == MSG_CLOSE)
				{
					error = ConnectionError.ConnectionError;
					break;
				}
			}
			if (error != ConnectionError.None)
				throw new ConnectionException(error);
		}
	}
}
