#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;

namespace MonoHaven.Network
{
	internal class MessageReceiver : BackgroundTask
	{
		private readonly Action<MessageReader> messageHandler;
		private readonly GameSocket socket;

		public MessageReceiver(GameSocket socket, Action<MessageReader> handler)
			: base("Message Receiver")
		{
			this.socket = socket;
			this.messageHandler = handler;
		}

		protected override void OnStart()
		{
			MessageReader message;
			while (!IsCancelled)
			{
				if (socket.Receive(out message))
					messageHandler(message);
			}
		}
	}
}
