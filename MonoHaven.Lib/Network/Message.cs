using System;
using System.Drawing;
using System.IO;
using System.Text;
using MiscUtil.Collections.Extensions;
using MiscUtil.Conversion;

namespace MonoHaven.Network
{
	public class Message : IDisposable
	{
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

		public void AddBytes(byte[] src, int off, int len)
		{
			stream.Write(src, off, len);
		}

		public void AddBytes(byte[] src)
		{
			AddBytes(src, 0, src.Length);
		}

		public void AddUint8(byte value)
		{
			AddBytes(EndianBitConverter.Little.GetBytes(value));
		}

		public void AddUint16(ushort value)
		{
			AddBytes(EndianBitConverter.Little.GetBytes(value));
		}

		public void AddInt32(int value)
		{
			AddBytes(EndianBitConverter.Little.GetBytes(value));
		}

		public void AddString(string str)
		{
			AddChars(str);
			AddBytes(new byte[] { 0 });
		}

		public void AddChars(char[] chars)
		{
			AddBytes(Encoding.UTF8.GetBytes(chars));
		}

		public void AddChars(string str)
		{
			AddBytes(Encoding.UTF8.GetBytes(str));
		}

		public void AddCoord(Point c)
		{
			AddInt32(c.X);
			AddInt32(c.Y);
		}

		public void AddList(params object[] args)
		{
			foreach (object o in args)
			{
				if (o is int)
				{
					AddUint8(Message.T_INT);
					AddInt32((int)o);
				}
				else if (o is string)
				{
					AddUint8(Message.T_STR);
					AddString((string)o);
				}
				else if (o is Point)
				{
					AddUint8(Message.T_COORD);
					AddCoord((Point)o);
				}
				else if (o != null)
				{
					throw new ArgumentException(string.Format(
						"Unsupported list element type: {0}",
						o.GetType()));
				}
				else
				{
					throw new ArgumentNullException(
						"args", "One of the arguments is null");
				}
			}
		}

		public byte[] GetBytes()
		{
			return stream.ToArray();
		}

		public void CopyBytesTo(byte[] buffer, int offset, int count)
		{
			var oldpos = stream.Position;
			stream.Position = 0;
			stream.Read(buffer, offset, count);
			stream.Position = oldpos;
		}
	}
}

