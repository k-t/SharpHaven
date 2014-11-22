#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using MonoHaven.Network;

namespace MonoHaven.Game.Messages
{
	public class WidgetDestroyMessage
	{
		public WidgetDestroyMessage(ushort id)
		{
			Id = id;
		}

		public ushort Id
		{
			get;
			private set;
		}

		public static WidgetDestroyMessage ReadFrom(MessageReader reader)
		{
			return new WidgetDestroyMessage(id: reader.ReadUint16());
		}
	}
}
