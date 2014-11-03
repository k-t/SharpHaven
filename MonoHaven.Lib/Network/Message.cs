using System;
using System.Text;

namespace MonoHaven.Network
{
	public class Message
	{
		public const int RMSG_NEWWDG = 0;
		public const int RMSG_WDGMSG = 1;
		public const int RMSG_DSTWDG = 2;
		public const int RMSG_MAPIV = 3;
		public const int RMSG_GLOBLOB = 4;
		public const int RMSG_PAGINAE = 5;
		public const int RMSG_RESID = 6;
		public const int RMSG_PARTY = 7;
		public const int RMSG_SFX = 8;
		public const int RMSG_CATTR = 9;
		public const int RMSG_MUSIC = 10;
		public const int RMSG_TILES = 11;
		public const int RMSG_BUFF = 12;

		public const int T_END = 0;
		public const int T_INT = 1;
		public const int T_STR = 2;
		public const int T_COORD = 3;
		public const int T_COLOR = 6;

		private readonly byte type;
		private readonly byte[] data;

		public Message(byte type, byte[] data)
		{
			this.type = type;
			this.data = data;
		}

		public Message(byte type, byte[] blob, int offset, int len)
		{
			if (blob == null)
				throw new ArgumentNullException("blob");

			this.type = type;
			this.data = new byte[len];

			Array.Copy(blob, offset, this.data, 0, len);
		}

		public byte[] Data
		{
			get { return data; }
		}

		public int Length
		{
			get { return data.Length; }
		}

		public int Type
		{
			get { return type; }
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Message))
				return false;

			Message m = (Message)obj;

			if (this.Length != m.Length)
				return false;

			for (int i = 0; i < this.Length; i++)
				if (this.Data[i] != m.Data[i])
					return false;

			return true;
		}

		public override int GetHashCode()
		{
			return this.Data.GetHashCode();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			foreach (byte b in this.Data)
			{
				sb.Append(string.Format("{0:X2} ", b));
			}
			return "Message(" + type + "): " + sb;
		}
	}
}

