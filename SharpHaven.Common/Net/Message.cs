using System;
using System.Text;
using MiscUtil.Conversion;
using SharpHaven.Graphics;
using SharpHaven.Utils;

namespace SharpHaven.Net
{
	public class Message : IDisposable
	{
		public const int MSG_SESS = 0;
		public const int MSG_REL = 1;
		public const int MSG_ACK = 2;
		public const int MSG_BEAT = 3;
		public const int MSG_MAPREQ = 4;
		public const int MSG_MAPDATA = 5;
		public const int MSG_OBJDATA = 6;
		public const int MSG_OBJACK = 7;
		public const int MSG_CLOSE = 8;

		private readonly byte type;
		private readonly ByteBuffer buffer;

		public Message(byte type, byte[] buffer)
			: this(type, buffer, 0, buffer.Length)
		{
		}

		public Message(byte type, byte[] buffer, int offset, int length)
			: this(type, new ByteBuffer(buffer, offset, length))
		{
		}

		public Message(byte type)
			: this(type, new ByteBuffer())
		{
		}

		public Message(byte type, ByteBuffer buffer)
		{
			this.type = type;
			this.buffer = buffer;
		}

		public ByteBuffer Buffer
		{
			get { return buffer; }
		}

		public int Length
		{
			get { return (int)buffer.Length; }
		}

		public byte Type
		{
			get { return type; }
		}

		public void Dispose()
		{
			buffer.Dispose();
		}

		public Message Bytes(byte[] src, int off, int len)
		{
			buffer.Write(src, off, len);
			return this;
		}

		public Message Bytes(byte[] src)
		{
			Bytes(src, 0, src.Length);
			return this;
		}

		public Message Byte(byte value)
		{
			buffer.Write(value);
			return this;
		}

		public Message Uint16(ushort value)
		{
			Bytes(EndianBitConverter.Little.GetBytes(value));
			return this;
		}

		public Message UInt32(uint value)
		{
			Bytes(EndianBitConverter.Little.GetBytes(value));
			return this;
		}

		public Message Int32(int value)
		{
			Bytes(EndianBitConverter.Little.GetBytes(value));
			return this;
		}

		public Message String(string str)
		{
			Chars(str);
			Bytes(new byte[] { 0 });
			return this;
		}

		public Message Chars(char[] chars)
		{
			Bytes(Encoding.UTF8.GetBytes(chars));
			return this;
		}

		public Message Chars(string str)
		{
			Bytes(Encoding.UTF8.GetBytes(str));
			return this;
		}

		public Message Coord(int x, int y)
		{
			Int32(x);
			Int32(y);
			return this;
		}

		public Message Coord(Coord2D c)
		{
			return Coord(c.X, c.Y);
		}

		public Message List(params object[] args)
		{
			buffer.WriteList(args);
			return this;
		}
	}
}

