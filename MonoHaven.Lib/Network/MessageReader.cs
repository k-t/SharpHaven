using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MiscUtil.Conversion;

namespace MonoHaven.Network
{
	public class MessageReader
	{
		private readonly Message message;
		private int off;

		public MessageReader(Message message)
		{
			if (message == null)
				throw new ArgumentNullException("message");

			this.message = message;
		}

		public bool IsEom
		{
			get { return off >= message.Length; }
		}
		
		public sbyte Int8()
		{
			return (sbyte)message.Data[off++];
		}

		public byte ReadUint8()
		{
			return message.Data[off++];
		}

		public ushort ReadUint16()
		{
			off += 2;
			return EndianBitConverter.Little.ToUInt16(message.Data, off - 2);
		}

		public int ReadInt32()
		{
			off += 4;
			return EndianBitConverter.Little.ToInt32(message.Data, off - 4);
		}

		public string ReadString()
		{
			int start = off;
			int end = message.Data.Length;
			for (int i = start; i < message.Data.Length; i++)
				if (message.Data[i] == 0)
				{
					end = i;
					break;
				}
			off = end + 1;
			return Encoding.UTF8.GetString(message.Data, start, end - start);
		}

		public Point ReadCoord()
		{
			return new Point(ReadInt32(), ReadInt32());
		}

		public Color ReadColor()
		{
			int r = ReadUint8();
			int g = ReadUint8();
			int b = ReadUint8();
			int a = ReadUint8();
			return Color.FromArgb(a, r, g, b);
		}

		public object[] ReadList()
		{
			List<object> ret = new List<object>();

			while (true)
			{
				if (off >= message.Length)
					break;

				int t = ReadUint8();
				switch (t)
				{
					case Message.T_INT:
						ret.Add(ReadInt32());
						break;
					case Message.T_STR:
						ret.Add(ReadString());
						break;
					case Message.T_COLOR:
						ret.Add(ReadColor());
						break;
					case Message.T_COORD:
						ret.Add(ReadCoord());
						break;
					case Message.T_END:
						break;
				}
			}

			return ret.ToArray();
		}
	}
}

