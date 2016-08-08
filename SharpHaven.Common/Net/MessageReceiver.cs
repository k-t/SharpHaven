﻿using System;
using NLog;
using SharpHaven.Utils;

namespace SharpHaven.Net
{
	internal class MessageReceiver : BackgroundTask
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly Action<BinaryMessage> messageHandler;
		private readonly BinaryMessageSocket socket;

		public MessageReceiver(BinaryMessageSocket socket, Action<BinaryMessage> handler)
			: base("Message Receiver")
		{
			this.socket = socket;
			this.messageHandler = handler;
		}

		protected override void OnStart()
		{
			BinaryMessage message;
			try
			{
				while (!IsCancelled)
				{
					if (socket.Receive(out message))
						messageHandler(message);
				}
			}
			catch (ObjectDisposedException)
			{
				// to prevent this exception:
				// a. code that stops tasks needs to be written more carefully
				// b. socket poll method needs to support cancellation
				Log.Info("Socket was disposed while polling");
			}
		}
	}
}
