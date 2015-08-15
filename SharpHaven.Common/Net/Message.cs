using System;
using System.Drawing;
using System.IO;
using System.Text;
using MiscUtil.Conversion;

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

		public const byte T_END = 0;
		public const byte T_INT = 1;
		public const byte T_STR = 2;
		public const byte T_COORD = 3;
		public const byte T_COLOR = 6;

		private readonly byte type;
		private readonly MemoryStream stream;

		public Message(byte type)
		{
			this.type = type;
			this.stream = new MemoryStream();
		}

		public int Length
		{
			get { return (int)stream.Length; }
		}

		public byte Type
		{
			get { return type; }
		}

		public void Dispose()
		{
			stream.Dispose();
		}

		public Message Bytes(byte[] src, int off, int len)
		{
			stream.Write(src, off, len);
			return this;
		}

		public Message Bytes(byte[] src)
		{
			Bytes(src, 0, src.Length);
			return this;
		}

		public Message Byte(byte value)
		{
			stream.WriteByte(value);
			return this;
		}

		public Message Uint16(ushort value)
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

		public Message Coord(Point c)
		{
			return Coord(c.X, c.Y);
		}

		public Message List(params object[] args)
		{
			foreach (object o in args)
			{
				if (o is int)
				{
					Byte(Message.T_INT);
					Int32((int)o);
				}
				else if (o is string)
				{
					Byte(Message.T_STR);
					String((string)o);
				}
				else if (o is Point)
				{
					Byte(Message.T_COORD);
					Coord((Point)o);
				}
				else if (o != null)
				{
					throw new ArgumentException($"Unsupported list element type: {o.GetType()}");
				}
				else
				{
					throw new ArgumentNullException(nameof(args), "One of the arguments is null");
				}
			}
			return this;
		}

		public byte[] GetAllBytes()
		{
			return stream.ToArray();
		}

		public void CopyBytes(byte[] buffer, int offset, int count)
		{
			var oldpos = stream.Position;
			stream.Position = 0;
			stream.Read(buffer, offset, count);
			stream.Position = oldpos;
		}
	}
}

