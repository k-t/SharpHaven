using System.Drawing;

namespace SharpHaven.Client
{
	public class GobFollowing
	{
		private readonly Gob gob;
		private readonly Point offset;
		private readonly byte szo;

		public GobFollowing(Gob gob, Point offset, byte szo)
		{
			this.gob = gob;
			this.offset = offset;
			this.szo = szo;
		}

		public Gob Gob
		{
			get { return gob; }
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
