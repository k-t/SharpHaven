using System;
using System.Collections.Generic;
using System.Threading;
using MonoHaven.Utils;

namespace MonoHaven.Network
{
	internal class MessageSender : BackgroundTask
	{
		private const int KeepAliveTimeout = 5000;
		private const int AckThreshold = 30;

		private readonly object thisLock = new object();
		private readonly GameSocket socket;
		private readonly List<PendingMessage> pending;
		private ushort pendingSeq;
		private ushort ackSeq;
		private DateTime? ackTime;

		public MessageSender(GameSocket socket)
			: base("Message Sender")
		{
			this.socket = socket;
			this.pending = new List<PendingMessage>();
		}

		protected override void OnStart()
		{
			var last = DateTime.Now;
			while (!IsCancelled)
			{
				var to = TimeSpan.FromMilliseconds(5000);
				var now = DateTime.Now;
				var beat = true;

				lock (pending)
				{
					if(pending.Count > 0)
						to = TimeSpan.FromMilliseconds(60);
				}
				lock (thisLock)
				{
					if (ackTime.HasValue)
						to = ackTime.Value - now + TimeSpan.FromMilliseconds(AckThreshold);
					if(to.TotalMilliseconds > 0)
						Monitor.Wait(thisLock, to);
				}

				lock (pending)
				{
					foreach (var msg in pending)
					{
						int txtime;

						if (msg.RetryCount == 0)
							txtime = 0;
						else if (msg.RetryCount == 1)
							txtime = 80;
						else if (msg.RetryCount < 4)
							txtime = 200;
						else if (msg.RetryCount < 10)
							txtime = 620;
						else
							txtime = 2000;

						if ((now - msg.Last).TotalMilliseconds > txtime)
						{
							msg.Last = now;
							msg.RetryCount++;
							var rmsg = new Message(Message.MSG_REL)
								.Uint16(msg.Seq)
								.Byte(msg.Message.Type)
								.Bytes(msg.Message.GetAllBytes());
							socket.SendMessage(rmsg);
							beat = false;
						}
					}
				}
				
				lock (thisLock)
				{
					if (ackTime.HasValue && ((now - ackTime.Value).TotalMilliseconds >= AckThreshold))
					{
						socket.SendMessage(new Message(Message.MSG_ACK).Uint16(ackSeq));
						ackTime = null;
						beat = false;
					}
				}

				if (beat)
				{
					if ((now - last).TotalMilliseconds > KeepAliveTimeout)
					{
						socket.SendMessage(new Message(Message.MSG_BEAT));
						last = now;
					}
				}
			}
		}

		public void SendMessage(Message message)
		{
			lock (pending)
			{
				pending.Add(new PendingMessage(pendingSeq, message));
				pendingSeq++;
			}
			lock (thisLock)
				Monitor.PulseAll(thisLock);
		}

		public void SendAck(ushort seq)
		{
			lock (thisLock)
			{
				ackSeq = seq;
				if (!ackTime.HasValue)
					ackTime = DateTime.Now;
				Monitor.PulseAll(thisLock);
			}
		}

		public void ReceiveAck(ushort seq)
		{
			lock (pending)
				pending.RemoveAll(x => x.Seq <= seq);
		}

		private class PendingMessage
		{
			public PendingMessage(ushort seq, Message message)
			{
				Message = message;
				Seq = seq;
				Last = DateTime.Now;
				RetryCount = 0;
			}

			public Message Message { get; private set; }
			public ushort Seq { get; private set; }
			public DateTime Last { get; set; }
			public int RetryCount { get; set; }
		}
	}
}
