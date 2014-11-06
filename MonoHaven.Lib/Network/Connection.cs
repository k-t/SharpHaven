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
		private readonly Socket socket;
		private readonly MessageReceiver receiver;
		private readonly MessageSender sender;

		private ConnectionState state;
		private ConnectionError error;

		public Connection(ConnectionSettings settings)
		{
			this.settings = settings;

			state = ConnectionState.Created;
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			sender = new MessageSender(socket);
			receiver = new MessageReceiver(socket);
		}

		public void Open()
		{
			socket.Connect(settings.Host, settings.Port);
			receiver.SetHandler(EndHandshake);
			receiver.Start();
			sender.Start();
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
				var hello = new Message(MSG_SESS);
				hello.AddUint16(1);
				hello.AddString("Haven");
				hello.AddUint16(PROTOCOL_VERSION);
				hello.AddString(settings.UserName);
				hello.AddBytes(settings.Cookie);
				Send(hello);

				state = ConnectionState.Opening;
				while (state == ConnectionState.Opening)
					Monitor.Wait(connLock);

				if (error != 0)
					throw new ConnectionException(error, GetErrorMessage(error));
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

		private void Send(Message msg)
		{
			byte[] bytes = new byte[msg.Length + 1];
			bytes[0] = msg.Type;
			msg.CopyBytesTo(bytes, 1, msg.Length);
			socket.Send(bytes);
		}

		private static string GetErrorMessage(ConnectionError error)
		{
			switch (error)
			{
				case 0:
					return "";
				case ConnectionError.InvalidToken:
					return "Invalid authentication token";
				case ConnectionError.AlreadyLoggedIn:
					return "Already logged in";
				case ConnectionError.ConnectionError:
					return "Could not connect to server";
				case ConnectionError.InvalidProtocolVersion:
					return "This client is too old";
				case ConnectionError.ExpiredToken:
					return "Authentication token expired";
				default:
					return "Connection failed";
			}
		}
	}
}
