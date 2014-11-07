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
				throw new Exception("Couldn't receive data from socket");
			var type = receiveBuffer[0];
			var buffer = new byte[size - 1];
			Array.Copy(receiveBuffer, 1, buffer, 0, size - 1);
			return new MessageReader(type, buffer);
		}

		public void SendMessage(Message msg)
		{
			var buffer = new byte[msg.Length + 1];
			buffer[0] = msg.Type;
			msg.CopyBytes(buffer, 1, msg.Length);
			socket.Send(buffer);
		}
	}
}
