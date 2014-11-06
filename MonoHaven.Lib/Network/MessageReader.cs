using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MiscUtil.Conversion;

namespace MonoHaven.Network
{
	public class MessageReader
	{
		private readonly byte[] buffer;
		private readonly int offset;
		private readonly int length;
		private readonly byte messageType;
		private int position;

		public MessageReader(byte messageType, byte[] buffer)
			: this(messageType, buffer, 0, buffer.Length)
		{
		}

		public MessageReader(byte messageType, byte[] buffer, int offset, int length)
		{
			this.messageType = messageType;
			this.buffer = buffer;
			this.offset = offset;
			this.length = length;

			Position = offset;
		}

		public byte MessageType
		{
			get { return messageType; }
		}

		public bool IsEom
		{
			get { return Position >= offset + length; }
		}

		private int Position
		{
			get { return position; }
			set { position = value; }
		}
		
		public sbyte ReadSByte()
		{
			return (sbyte)buffer[Position++];
		}

		public byte ReadByte()
		{
			return buffer[Position++];
		}

		public ushort ReadUint16()
		{
			Position += 2;
			return EndianBitConverter.Little.ToUInt16(buffer, Position - 2);
		}

		public int ReadInt32()
		{
			Position += 4;
			return EndianBitConverter.Little.ToInt32(buffer, Position - 4);
		}

		public string ReadString()
		{
			int start = position;
			int end = offset + length;
			for (int i = start; i < end; i++)
				if (buffer[i] == 0)
				{
					end = i;
					break;
				}
			position = end + 1;
			return Encoding.UTF8.GetString(buffer, start, end - start);
		}

		public Point ReadCoord()
		{
			return new Point(ReadInt32(), ReadInt32());
		}

		public Color ReadColor()
		{
			int r = ReadByte();
			int g = ReadByte();
			int b = ReadByte();
			int a = ReadByte();
			return Color.FromArgb(a, r, g, b);
		}

		public object[] ReadList()
		{
			var list = new List<object>();
			while (true)
			{
				if (position >= offset + length)
					break;

				byte t = ReadByte();
				switch (t)
				{
					case Message.T_INT:
						list.Add(ReadInt32());
						break;
					case Message.T_STR:
						list.Add(ReadString());
						break;
					case Message.T_COLOR:
						list.Add(ReadColor());
						break;
					case Message.T_COORD:
						list.Add(ReadCoord());
						break;
					case Message.T_END:
						break;
				}
			}
			return list.ToArray();
		}

		public byte[] GetBytes()
		{
			var bytes = new byte[length];
			Array.Copy(buffer, offset, bytes, 0, length);
			return bytes;
		}
	}
}

