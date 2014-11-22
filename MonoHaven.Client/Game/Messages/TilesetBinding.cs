#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using MonoHaven.Network;

namespace MonoHaven.Game.Messages
{
	public class TilesetBinding
	{
		public TilesetBinding(byte id, string name, ushort version)
		{
			Id = id;
			Name = name;
			Version = version;
		}

		public byte Id
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public ushort Version
		{
			get;
			private set;
		}

		public static TilesetBinding ReadFrom(MessageReader reader)
		{
			return new TilesetBinding(
				id: reader.ReadByte(),
				name: reader.ReadString(),
				version: reader.ReadUint16()
			);
		}
	}
}
