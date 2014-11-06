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

		private readonly Object syncRoot = new object();
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
			receiver.SetHandler(ReceiveHandshake);
		}

		public void Open()
		{
			socket.Connect(settings.Host, settings.Port);

			sender.Start();
			receiver.Start();

			lock (syncRoot)
			{
				var hello = new MessageWriter(MSG_SESS);
				hello.AddUint16(1);
				hello.AddString("Haven");
				hello.AddUint16(PROTOCOL_VERSION);
				hello.AddString(settings.UserName);
				hello.AddBytes(settings.Cookie);
				Send(hello.GetMessage());

				state = ConnectionState.Opening;
				while (state == ConnectionState.Opening)
					Monitor.Wait(syncRoot);

				if (error != 0)
					throw new ConnectionException(error, GetErrorMessage(error));
			}
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

		private void ReceiveHandshake(Message message)
		{
			ConnectionError err;
			switch (message.Type)
			{
				case MSG_SESS:
					err = (ConnectionError)message.Data[0];
					break;
				case MSG_CLOSE:
					err = ConnectionError.ConnectionError;
					break;
				default:
					return;
			}
			lock (syncRoot)
			{
				state = err == 0 ? ConnectionState.Opened : ConnectionState.Closed;
				error = err;
				Monitor.PulseAll(syncRoot);
			}
		}

		private void Send(Message msg)
		{
			byte[] buf = new byte[msg.Length + 1];
			buf[0] = (byte)msg.Type;
			Array.Copy(msg.Data, 0, buf, 1, msg.Length);
			socket.Send(buf);
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
