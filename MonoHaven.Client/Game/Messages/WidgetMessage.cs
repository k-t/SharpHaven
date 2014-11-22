#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using MonoHaven.Network;

namespace MonoHaven.Game.Messages
{
	public class WidgetMessage
	{
		public WidgetMessage(ushort id, string name, object[] args)
		{
			Id = id;
			Name = name;
			Args = args;
		}

		public ushort Id
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public object[] Args
		{
			get;
			private set;
		}

		public static WidgetMessage ReadFrom(MessageReader reader)
		{
			return new WidgetMessage(
				id: reader.ReadUint16(),
				name: reader.ReadString(),
				args: reader.ReadList()
			);
		}
	}
}
