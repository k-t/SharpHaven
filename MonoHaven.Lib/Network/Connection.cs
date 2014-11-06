using System;
using System.Net.Sockets;
using System.Threading;

namespace MonoHaven.Network
{
	public class Connection : IDisposable
	{
		private const int PVER = 2;
		private const int MSG_SESS = 0;

		private readonly Object syncRoot = new object();
		private readonly ConnectionSettings settings;
		private readonly Socket socket;
		private readonly MessageReceiver receiver;
		private readonly MessageSender sender;

		private ConnectionState state;
		private ConnectionErrorCode errorCode;

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
				hello.AddUint16(PVER);
				hello.AddString(settings.UserName);
				hello.AddBytes(settings.Cookie);
				Send(hello.GetMessage());

				state = ConnectionState.Opening;
				while (state == ConnectionState.Opening)
					Monitor.Wait(syncRoot);

				if (errorCode != 0)
					throw new ConnectionException(errorCode, GetErrorMessage(errorCode));
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
			ConnectionErrorCode errorCode;
			
			if (message != null)
			{
				if (message.Type != MSG_SESS)
					return;
				errorCode = (ConnectionErrorCode)message.Data[0];
			}
			else
				errorCode = ConnectionErrorCode.ConnectionError;

			lock (syncRoot)
			{
				state = errorCode == 0
					? ConnectionState.Opened
					: ConnectionState.Closed;
				this.errorCode = errorCode;
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

		private static string GetErrorMessage(ConnectionErrorCode errorCode)
		{
			switch (errorCode)
			{
				case 0:
					return "";
				case ConnectionErrorCode.InvalidToken:
					return "Invalid authentication token";
				case ConnectionErrorCode.AlreadyLoggedIn:
					return "Already logged in";
				case ConnectionErrorCode.ConnectionError:
					return "Could not connect to server";
				case ConnectionErrorCode.InvalidProtocolVersion:
					return "This client is too old";
				case ConnectionErrorCode.ExpiredToken:
					return "Authentication token expired";
				default:
					return "Connection failed";
			}
		}
	}
}
