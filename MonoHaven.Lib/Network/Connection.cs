﻿using System;
using System.Net.Sockets;
using System.Threading;

namespace MonoHaven.Network
{
	public class Connection : IDisposable
	{
		private const int ProtocolVersion = 2;

		private const int MSG_SESS = 0;
		private const int MSG_CLOSE = 8;

		private readonly ConnectionSettings settings;
		private readonly GameSocket socket;
		private readonly MessageReceiver receiver;
		private readonly MessageSender sender;
		private ConnectionState state;
		private readonly object stateLock = new object();

		public Connection(ConnectionSettings settings)
		{
			this.settings = settings;

			state = ConnectionState.Created;
			socket = new GameSocket(settings.Host, settings.Port);
			sender = new MessageSender(socket);
			sender.Finished += TaskFinished;
			receiver = new MessageReceiver(socket);
			receiver.Finished += TaskFinished;
		}

		public void Dispose()
		{
			Close();
		}

		public void Open()
		{
			try
			{
				lock (stateLock)
				{
					if (state != ConnectionState.Created)
						throw new InvalidOperationException("Can't open already opened/closed connection");

					Connect();
					receiver.Run();
					sender.Run();

					state = ConnectionState.Opened;
				}
			}
			catch (SocketException ex)
			{
				throw new ConnectionException(ex.Message, ex);
			}
		}

		public void Close()
		{
			lock (stateLock)
			{
				if (state != ConnectionState.Opened)
					return;

				receiver.Stop();
				sender.Stop();

				socket.SendMessage(new Message(MSG_CLOSE));
				socket.Close();

				state = ConnectionState.Closed;
			}
		}

		private void Connect()
		{
			socket.Connect();

			var hello = new Message(MSG_SESS)
				.Uint16(1)
				.String("Haven")
				.Uint16(ProtocolVersion)
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

		private void TaskFinished(object sender, EventArgs args)
		{
			// TODO: call this method in another thread so it won't block task completion?
			Close();
		}
	}
}
