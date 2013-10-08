using System;
using System.IO;
using MiscUtil.Conversion;
using System.Text;
using System.Drawing;

namespace MonoHaven.Network
{
	public class MessageWriter : IDisposable
	{
		private readonly int messageType;
		private readonly MemoryStream stream;

		public MessageWriter(int messageType)
		{
			this.messageType = messageType;
			this.stream = new MemoryStream();
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
			AddString2(str);
			AddBytes(new byte[] { 0 });
		}

		public void AddString2(string str)
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

		public Message GetMessage()
		{
			return new Message(messageType, stream.GetBuffer());
		}

		public void Dispose()
		{
			stream.Dispose();
		}
	}
}

