using System.Text;
using MiscUtil.Conversion;
using SharpHaven.Graphics;
using SharpHaven.Utils;

namespace SharpHaven.Net
{
	public class BinaryMessageWriter
	{
		private readonly byte type;
		private readonly ByteBuffer buffer;

		public BinaryMessageWriter(byte type)
		{
			this.type = type;
			this.buffer = new ByteBuffer();
		}

		public BinaryMessageWriter Bytes(byte[] src, int off, int len)
		{
			buffer.Write(src, off, len);
			return this;
		}

		public BinaryMessageWriter Bytes(byte[] src)
		{
			Bytes(src, 0, src.Length);
			return this;
		}

		public BinaryMessageWriter Byte(byte value)
		{
			buffer.Write(value);
			return this;
		}

		public BinaryMessageWriter UInt16(ushort value)
		{
			Bytes(EndianBitConverter.Little.GetBytes(value));
			return this;
		}

		public BinaryMessageWriter UInt32(uint value)
		{
			Bytes(EndianBitConverter.Little.GetBytes(value));
			return this;
		}

		public BinaryMessageWriter Int32(int value)
		{
			Bytes(EndianBitConverter.Little.GetBytes(value));
			return this;
		}

		public BinaryMessageWriter String(string str)
		{
			Chars(str);
			Bytes(new byte[] { 0 });
			return this;
		}

		public BinaryMessageWriter Chars(char[] chars)
		{
			Bytes(Encoding.UTF8.GetBytes(chars));
			return this;
		}

		public BinaryMessageWriter Chars(string str)
		{
			Bytes(Encoding.UTF8.GetBytes(str));
			return this;
		}

		public BinaryMessageWriter Coord(int x, int y)
		{
			Int32(x);
			Int32(y);
			return this;
		}

		public BinaryMessageWriter Coord(Coord2D c)
		{
			return Coord(c.X, c.Y);
		}

		public BinaryMessageWriter List(params object[] args)
		{
			buffer.WriteList(args);
			return this;
		}

		public BinaryMessage Complete()
		{
			buffer.Rewind();
			var bytes = buffer.ReadRemaining();
			return new BinaryMessage(type, bytes);
		}
	}
}
