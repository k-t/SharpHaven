using System;
using System.Net;
using System.Net.Sockets;

namespace MonoHaven.Network
{

	public class GameSocket : IDisposable
	{
		private const int ReceiveTimeout = 1000; // 1 sec

		private readonly EndPoint address;
		private readonly Socket socket;
		private readonly byte[] receiveBuffer;

		public GameSocket(string host, int port)
		{
			address = new DnsEndPoint(host, port);
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			socket.ReceiveTimeout = ReceiveTimeout;
			receiveBuffer = new byte[socket.ReceiveBufferSize];
		}

		public void Dispose()
		{
			Close();
		}

		public void Connect()
		{
			socket.Connect(address);
		}

		public void Close()
		{
			socket.Close();
		}

		public MessageReader ReceiveMessage()
		{
			int size = socket.Receive(receiveBuffer);
			if (size == 0)
				throw new InvalidOperationException("Socket is closed");
			var type = receiveBuffer[0];
			var blob = new byte[size - 1];
			Array.Copy(receiveBuffer, 1, blob, 0, size - 1);
			return new MessageReader(type, blob);
		}

		public void SendMessage(Message msg)
		{
			var bytes = new byte[msg.Length + 1];
			bytes[0] = msg.Type;
			msg.CopyBytes(bytes, 1, msg.Length);
			socket.Send(bytes);
		}
	}
}
