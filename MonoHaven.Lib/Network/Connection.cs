using System;
using System.Net.Sockets;
using C5;
using NLog;
using MonoHaven.Utils;

namespace MonoHaven.Network
{
	public class Connection : IDisposable
	{
		#region Constants

		private const int ReceiveTimeout = 1000; // milliseconds

		private const int ProtocolVersion = 2;

		private const int MSG_SESS = 0;
		private const int MSG_REL = 1;
		private const int MSG_ACK = 2;
		private const int MSG_MAPREQ = 4;
		private const int MSG_MAPDATA = 5;
		private const int MSG_OBJDATA = 6;
		private const int MSG_OBJACK = 7;
		private const int MSG_CLOSE = 8;

		#endregion

		private static readonly NLog.Logger log = LogManager.GetCurrentClassLogger();

		private readonly ConnectionSettings settings;
		private readonly GameSocket socket;
		private readonly MessageReceiver receiver;
		private readonly MessageSender sender;
		private ConnectionState state;
		private readonly object stateLock = new object();
		private ushort rseq;
		private readonly TreeDictionary<ushort, MessageReader> waiting;

		public Connection(ConnectionSettings settings)
		{
			this.settings = settings;

			waiting = new TreeDictionary<ushort, MessageReader>();

			state = ConnectionState.Created;
			socket = new GameSocket(settings.Host, settings.Port);
			socket.SetReceiveTimeout(ReceiveTimeout);
			sender = new MessageSender(socket);
			sender.Finished += OnTaskFinished;
			receiver = new MessageReceiver(socket, ReceiveMessage);
			receiver.Finished += OnTaskFinished;
		}

		public event Action Closed;
		public event Action<MessageReader> MessageReceived;
		public event Action<MessageReader> MapDataReceived;

		public void Dispose()
		{
			Close();
		}

		public void Open()
		{
			try
			{
				lock (stateLock)
				{
					if (state != ConnectionState.Created)
						throw new InvalidOperationException("Can't open already opened/closed connection");

					Connect();
					receiver.Run();
					sender.Run();

					state = ConnectionState.Opened;
				}
			}
			catch (SocketException ex)
			{
				throw new ConnectionException(ex.Message, ex);
			}
		}

		public void Close()
		{
			lock (stateLock)
			{
				if (state != ConnectionState.Opened)
					return;

				receiver.Finished -= OnTaskFinished;
				receiver.Stop();

				sender.Finished -= OnTaskFinished;
				sender.Stop();

				socket.SendMessage(new Message(MSG_CLOSE));
				socket.Close();

				state = ConnectionState.Closed;
			}
			Closed.Raise();
		}

		public void SendMessage(Message message)
		{
			sender.QueueMessage(message);
		}

		public void RequestMapData(int x, int y)
		{
			var msg = new Message(MSG_MAPREQ).Coord(x, y);
			socket.SendMessage(msg);
		}

		private void Connect()
		{
			socket.Connect();

			var hello = new Message(MSG_SESS)
				.Uint16(1)
				.String("Haven")
				.Uint16(ProtocolVersion)
				.String(settings.UserName)
				.Bytes(settings.Cookie);

			socket.SendMessage(hello);

			MessageReader reply;
			ConnectionError error;
			while (true)
			{
				if (!socket.Receive(out reply))
					throw new ConnectionException(ConnectionError.ConnectionError);

				if (reply.MessageType == MSG_SESS)
				{
					error = (ConnectionError)reply.ReadByte();
					break;
				}
				if (reply.MessageType == MSG_CLOSE)
				{
					error = ConnectionError.ConnectionError;
					break;
				}
			}
			if (error != ConnectionError.None)
				throw new ConnectionException(error);
		}

		private void ReceiveMessage(MessageReader msg)
		{
			switch (msg.MessageType)
			{
				case MSG_REL:
					var seq = msg.ReadUint16();
					while (!msg.IsEom)
					{
						var type = msg.ReadByte();
						int len;
						if ((type & 0x80) != 0) // is not last?
						{
							type &= 0x7f;
							len = msg.ReadUint16();
						}
						else
							len = msg.Length - msg.Position;
						HandleRel(seq, new MessageReader(type, msg, msg.Position, len));
						msg.Position += len;
						seq++;
					}
					break;
				case MSG_ACK:
					break;
				case MSG_MAPDATA:
					MapDataReceived.Raise(msg);
					break;
				case MSG_OBJDATA:
					break;
				case MSG_CLOSE:
					log.Info("Server dropped connection");
					Close();
					return;
			}
		}

		private void HandleRel(ushort seq, MessageReader msg)
		{
			if (seq == rseq)
			{
				MessageReceived.Raise(msg);
				while (true)
				{
					rseq++;
					if (!waiting.Remove(rseq, out msg))
						break;
					MessageReceived.Raise(msg);
				}
				SendAck((ushort)(rseq - 1));
			}
			else if (seq > rseq)
				waiting.Add(seq, msg);
		}

		private void SendAck(ushort seq)
		{
			// FIXME: it sending ack for each received message
			socket.SendMessage(new Message(MSG_ACK).Uint16(seq));
		}

		private void OnTaskFinished(object sender, EventArgs args)
		{
			// it shouldn't happen normally, so let it crash
			throw new Exception("Task finished abruptly");
		}
	}
}
