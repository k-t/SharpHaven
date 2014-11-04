using System;
using System.Net.Sockets;
using System.Threading;

namespace MonoHaven.Network
{
	public class GameClient : IDisposable
	{
		private const int SocketTimeout = 10000000; // 1 sec

		private const int PVER = 2;
		private const int MSG_SESS = 0;

		private readonly Object syncRoot = new object();
		private readonly Socket socket;
		private readonly Receiver receiver;
		private readonly Sender sender;
		private GameClientState state;
		private ConnectResult connectResult;

		public GameClient(string host, int port)
		{
			state = GameClientState.Created;
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			socket.Connect(host, port);
			sender = new Sender(this);
			receiver = new Receiver(this);
		}

		public ConnectResult Connect(string userName, byte[] cookie)
		{
			sender.Start();
			receiver.Start();

			lock (syncRoot)
			{
				var hello = new MessageWriter(MSG_SESS);
				hello.AddUint16(1);
				hello.AddString("Haven");
				hello.AddUint16(PVER);
				hello.AddString(userName);
				hello.AddBytes(cookie);
				sender.Send(hello.GetMessage());

				state = GameClientState.Connecting;
				while (state == GameClientState.Connecting)
					Monitor.Wait(syncRoot);
				return connectResult;
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

		class Receiver : NetworkLoop
		{
			private readonly byte[] receiveBuffer;
			private readonly GameClient client;

			public Receiver(GameClient client)
				: base("Message Receiver")
			{
				this.client = client;
				receiveBuffer = new byte[client.socket.ReceiveBufferSize];
			}

			protected override void Run()
			{
				Connect();
				Loop();
			}

			private void Connect()
			{
				ConnectResult connectResult;
				while (!IsCancelled)
				{
					var message = ReceiveMessage();
					if (message != null)
					{
						if (message.Type != MSG_SESS)
							continue;
						connectResult = (ConnectResult)message.Data[0];
					}
					else
						connectResult = ConnectResult.ConnectionFailed;

					lock (client.syncRoot)
					{
						client.state = connectResult == ConnectResult.Ok
							? GameClientState.Connected
							: GameClientState.Closed;
						client.connectResult = connectResult;
						Monitor.PulseAll(client.syncRoot);
					}
					break;
				}
			}

			private void Loop()
			{
				while (!IsCancelled && client.socket.Poll(SocketTimeout, SelectMode.SelectRead))
				{
					client.socket.Receive(receiveBuffer);
				}
			}

			private Message ReceiveMessage()
			{
				while (client.socket.Poll(SocketTimeout, SelectMode.SelectRead))
				{
					int size = client.socket.Receive(receiveBuffer);
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

		class Sender : NetworkLoop
		{
			private readonly GameClient client;

			public Sender(GameClient client)
				: base("Message Sender")
			{
				this.client = client;
			}

			public void Send(Message msg)
			{
				byte[] buf = new byte[msg.Length + 1];
				buf[0] = (byte)msg.Type;
				Array.Copy(msg.Data, 0, buf, 1, msg.Length);
				client.socket.Send(buf);
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
