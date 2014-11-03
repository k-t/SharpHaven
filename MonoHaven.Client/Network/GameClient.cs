using System;
using System.Net.Sockets;

namespace MonoHaven.Network
{
	public class GameClient : IDisposable
	{
		private const int SocketTimeout = 10000000; // 1 sec

		private const int PVER = 2;
		private const int MSG_SESS = 0;

		private readonly Socket socket;
		private readonly byte[] receiveBuffer;

		public GameClient(string host, int port)
		{
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			socket.Connect(host, port);
			receiveBuffer = new byte[socket.ReceiveBufferSize];
		}

		public ConnectResult Connect(string userName, byte[] cookie)
		{
			var input = new MessageWriter(MSG_SESS);
			input.AddUint16(1);
			input.AddString("Haven");
			input.AddUint16(PVER);
			input.AddString(userName);
			input.AddBytes(cookie);
			SendMessage(input.GetMessage());

			while (socket.Poll(SocketTimeout, SelectMode.SelectRead))
			{
				var output = ReceiveMessage();
				if (output.Type == MSG_SESS)
				{
					var reader = new MessageReader(output);
					return (ConnectResult)reader.ReadUint8();
				}
			}

			throw new Exception("Connection timeout");
		}

		public void Dispose()
		{
			if (socket != null)
				socket.Close();
		}

		private Message ReceiveMessage()
		{
			int size = socket.Receive(receiveBuffer);
			if (size == 0)
				throw new InvalidOperationException("Connection is closed");
			var type = receiveBuffer[0];
			var blob = new byte[size - 1];
			Array.Copy(receiveBuffer, 1, blob, 0, size - 1);
			return new Message(type, blob);
		}

		private void SendMessage(Message msg)
		{
			byte[] buf = new byte[msg.Length + 1];
			buf[0] = (byte)msg.Type;
			Array.Copy(msg.Data, 0, buf, 1, msg.Length);
			socket.Send(buf);
		}
	}
}
