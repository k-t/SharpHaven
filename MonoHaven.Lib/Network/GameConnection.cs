using System;
using System.Net.Sockets;
using System.Threading;

namespace MonoHaven.Network
{
	public class GameConnection : IDisposable
	{
		private const int SocketTimeout = 10000000; // 1 sec

		private const int PVER = 2;
		private const int MSG_SESS = 0;

		private readonly Object syncRoot = new object();
		private readonly ConnectionSettings settings;
		private readonly Socket socket;
		private readonly Receiver receiver;
		private readonly Sender sender;
		private ConnectionState state;
		private ConnectionErrorCode errorCode;

		public GameConnection(ConnectionSettings settings)
		{
			this.settings = settings;

			state = ConnectionState.Created;
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			sender = new Sender(this);
			receiver = new Receiver(this);
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
				sender.Send(hello.GetMessage());

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

		class Receiver : ConnectionWorker
		{
			private readonly byte[] receiveBuffer;

			public Receiver(GameConnection connection)
				: base("Message Receiver", connection)
			{
				receiveBuffer = new byte[connection.socket.ReceiveBufferSize];
			}

			protected override void Run()
			{
				Connect();
				Loop();
			}

			private void Connect()
			{
				ConnectionErrorCode errorCode;
				while (!IsCancelled)
				{
					var message = ReceiveMessage();
					if (message != null)
					{
						if (message.Type != MSG_SESS)
							continue;
						errorCode = (ConnectionErrorCode)message.Data[0];
					}
					else
						errorCode = ConnectionErrorCode.ConnectionError;

					lock (Connection.syncRoot)
					{
						Connection.state = errorCode == 0
							? ConnectionState.Opened
							: ConnectionState.Closed;
						Connection.errorCode = errorCode;
						Monitor.PulseAll(Connection.syncRoot);
					}
					break;
				}
			}

			private void Loop()
			{
				while (!IsCancelled && Connection.socket.Poll(SocketTimeout, SelectMode.SelectRead))
				{
					Connection.socket.Receive(receiveBuffer);
				}
			}

			private Message ReceiveMessage()
			{
				while (Connection.socket.Poll(SocketTimeout, SelectMode.SelectRead))
				{
					int size = Connection.socket.Receive(receiveBuffer);
					if (size == 0)
						throw new InvalidOperationException("Connection is closed");
					var type = receiveBuffer[0];
					var blob = new byte[size - 1];
					Array.Copy(receiveBuffer, 1, blob, 0, size - 1);
					return new Message(type, blob);
				}
				return null;
			}
		}

		class Sender : ConnectionWorker
		{
			public Sender(GameConnection connection)
				: base("Message Sender", connection)
			{
			}

			public void Send(Message msg)
			{
				byte[] buf = new byte[msg.Length + 1];
				buf[0] = (byte)msg.Type;
				Array.Copy(msg.Data, 0, buf, 1, msg.Length);
				Connection.socket.Send(buf);
			}

			protected override void Run()
			{
				Loop();
			}

			private void Loop()
			{
				while (!IsCancelled)
				{
				}
			}
		}
	}
}
