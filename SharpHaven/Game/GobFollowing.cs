using System.Drawing;

namespace SharpHaven.Game
{
	public class GobFollowing
	{
		private readonly int gobId;
		private readonly Point offset;
		private readonly byte szo;

		public GobFollowing(int gobId, Point offset, byte szo)
		{
			this.gobId = gobId;
			this.offset = offset;
			this.szo = szo;
		}

		public int GobId
		{
			get { return gobId; }
		}

		public Point Offset
		{
			get { return offset; }
		}

		public byte Szo
		{
			get { return szo; }
		}
	}
}
