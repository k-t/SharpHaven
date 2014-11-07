using System;
using System.Net.Sockets;
using System.Threading;

namespace MonoHaven.Network
{
	public class Connection : IDisposable
	{
		private const int PROTOCOL_VERSION = 2;

		private const int MSG_SESS = 0;
		private const int MSG_CLOSE = 8;

		private readonly Object connLock = new object();
		private readonly ConnectionSettings settings;
		private readonly GameSocket socket;
		private readonly MessageReceiver receiver;
		private readonly MessageSender sender;

		private ConnectionState state;
		private ConnectionError error;

		public Connection(ConnectionSettings settings)
		{
			this.settings = settings;

			state = ConnectionState.Created;
			socket = new GameSocket(settings.Host, settings.Port);
			sender = new MessageSender(socket);
			receiver = new MessageReceiver(socket);
		}

		public void Open()
		{
			socket.Connect();
			BeginHandshake();
		}

		public void Close()
		{
			receiver.Stop();
			sender.Stop();
			socket.Close();
		}

		public void Dispose()
		{
			Close();
		}

		private void BeginHandshake()
		{
			lock (connLock)
			{
				receiver.SetHandler(EndHandshake);
				receiver.Start();
				sender.Start();

				var hello = new Message(MSG_SESS)
					.Uint16(1)
					.String("Haven")
					.Uint16(PROTOCOL_VERSION)
					.String(settings.UserName)
					.Bytes(settings.Cookie);
				socket.SendMessage(hello);

				state = ConnectionState.Opening;
				while (state == ConnectionState.Opening)
					Monitor.Wait(connLock);

				if (error != ConnectionError.None)
					throw new ConnectionException(error);
			}
		}

		private void EndHandshake(MessageReader reader)
		{
			ConnectionError err;
			switch (reader.MessageType)
			{
				case MSG_SESS:
					err = (ConnectionError)reader.ReadByte();
					break;
				case MSG_CLOSE:
					err = ConnectionError.ConnectionError;
					break;
				default:
					return;
			}
			lock (connLock)
			{
				state = err == 0 ? ConnectionState.Opened : ConnectionState.Closed;
				error = err;
				Monitor.PulseAll(connLock);
			}
		}
	}
}
