﻿using System;
using System.Net;
using System.Net.Sockets;
using SharpHaven.Utils;

namespace SharpHaven.Net
{
	public class MessageSocket : IDisposable
	{
		private readonly EndPoint address;
		private readonly Socket socket;
		private readonly byte[] receiveBuffer;
		private int receiveTimeout;

		public MessageSocket(string host, int port)
		{
			address = new IPEndPoint(Dns.GetHostEntry(host).AddressList[0], port);
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			receiveBuffer = new byte[socket.ReceiveBufferSize];
		}

		public void Dispose()
		{
			socket.Close();
		}

		public void Connect()
		{
			socket.Connect(address);
		}

		public void Close()
		{
			socket.Close();
		}

		public bool Receive(out Message message)
		{
			if (!socket.Poll(receiveTimeout * 1000, SelectMode.SelectRead))
			{
				message = null;
				return false;
			}

			int size = socket.Receive(receiveBuffer);
			if (size == 0)
				throw new Exception("Couldn't receive data from socket");
			var type = receiveBuffer[0];
			var buffer = new byte[size - 1];
			Array.Copy(receiveBuffer, 1, buffer, 0, size - 1);

			message = new Message(type, buffer);
			return true;
		}

		public void Send(Message message)
		{
			var buffer = new byte[message.Length + 1];
			buffer[0] = message.Type;
			message.Buffer.Rewind();
			Array.Copy(message.Buffer.ReadRemaining(), 0, buffer, 1, message.Length);
			socket.Send(buffer);
		}

		public void SetReceiveTimeout(int milliseconds)
		{
			receiveTimeout = milliseconds * 1000;
		}
	}
}