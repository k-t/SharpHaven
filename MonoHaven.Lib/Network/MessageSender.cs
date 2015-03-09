using System;
using System.Collections.Generic;
using System.Threading;

namespace MonoHaven.Network
{
	internal class MessageSender : BackgroundTask
	{
		private const int KeepAliveTimeout = 5000;
		private const int AckThreshold = 30;

		// TODO: remove duplication
		private const int MSG_REL = 1;
		private const int MSG_ACK = 2;
		private const int MSG_BEAT = 3;

		private readonly object thisLock = new object();
		private readonly GameSocket socket;
		private readonly List<Message> pending;
		private ushort seq;
		private ushort ackSeq;
		private DateTime? ackTime;

		public MessageSender(GameSocket socket)
			: base("Message Sender")
		{
			this.socket = socket;
			this.pending = new List<Message>();
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
					foreach (var message in pending)
					{
						var rmsg = new Message(MSG_REL)
							.Uint16(seq)
							.Byte(message.Type)
							.Bytes(message.GetAllBytes());
						socket.SendMessage(rmsg);
						seq++;
						beat = false;
					}
					pending.Clear();
				}
				lock (thisLock)
				{
					if (ackTime.HasValue && ((now - ackTime.Value).TotalMilliseconds >= AckThreshold))
					{
						socket.SendMessage(new Message(MSG_ACK).Uint16(ackSeq));
						ackTime = null;
						beat = false;
					}
				}

				if (beat)
				{
					if ((now - last).TotalMilliseconds > KeepAliveTimeout)
					{
						socket.SendMessage(new Message(MSG_BEAT));
						last = now;
					}
				}
			}
		}

		public void SendMessage(Message message)
		{
			lock (pending)
				pending.Add(message);
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
	}
}
