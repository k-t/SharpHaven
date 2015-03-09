using System;
using System.Net.Sockets;
using C5;
using NLog;

namespace MonoHaven.Network
{
	public class Connection : IDisposable
	{
		#region Constants

		private const int ReceiveTimeout = 1000; // milliseconds
		private const int ProtocolVersion = 2;

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
		private readonly TreeDictionary<int, FragmentBuffer> mapFrags;

		public Connection(ConnectionSettings settings)
		{
			this.settings = settings;

			waiting = new TreeDictionary<ushort, MessageReader>();
			mapFrags = new TreeDictionary<int, FragmentBuffer>();

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
		public event Action<MessageReader> ObjDataReceived;

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

				socket.SendMessage(new Message(Message.MSG_CLOSE));
				socket.Close();

				state = ConnectionState.Closed;
			}
			Closed.Raise();
		}

		public void SendMessage(Message message)
		{
			sender.SendMessage(message);
		}

		public void RequestMapData(int x, int y)
		{
			var msg = new Message(Message.MSG_MAPREQ).Coord(x, y);
			socket.SendMessage(msg);
		}

		public void SendObjectAck(int id, int frame)
		{
			// FIXME: make it smarter
			socket.SendMessage(new Message(Message.MSG_OBJACK).Int32(id).Int32(frame));
		}

		private void Connect()
		{
			socket.Connect();

			var hello = new Message(Message.MSG_SESS)
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

				if (reply.MessageType == Message.MSG_SESS)
				{
					error = (ConnectionError)reply.ReadByte();
					break;
				}
				if (reply.MessageType == Message.MSG_CLOSE)
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
				case Message.MSG_REL:
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
				case Message.MSG_ACK:
					sender.ReceiveAck(msg.ReadUint16());
					break;
				case Message.MSG_MAPDATA:
					HandleMapData(msg);
					break;
				case Message.MSG_OBJDATA:
					while (msg.Position < msg.Length)
						ObjDataReceived.Raise(msg);
					break;
				case Message.MSG_CLOSE:
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
				sender.SendAck((ushort)(rseq - 1));
			}
			else if (seq > rseq)
				waiting[seq] = msg;
		}

		// TODO: delete lingering fragments?
		private void HandleMapData(MessageReader msg)
		{
			int packetId = msg.ReadInt32();
			int offset = msg.ReadUint16();
			int length = msg.ReadUint16();

			FragmentBuffer fragbuf;
			if (mapFrags.Find(ref packetId, out fragbuf))
			{
				fragbuf.Add(offset, msg.Buffer, 8, msg.Length - 8);
				if (fragbuf.IsComplete)
				{
					mapFrags.Remove(packetId);
					MapDataReceived.Raise(new MessageReader(0, fragbuf.Content));
				}
			}
			else if (offset != 0 || msg.Length - 8 < length)
			{
				fragbuf = new FragmentBuffer(length);
				fragbuf.Add(offset, msg.Buffer, 8, msg.Length - 8);
				mapFrags[packetId] = fragbuf;
			}
			else
				MapDataReceived.Raise(msg);
		}

		private void OnTaskFinished(object sender, EventArgs args)
		{
			// it shouldn't happen normally, so let it crash
			throw new Exception("Task finished abruptly");
		}
	}
}
