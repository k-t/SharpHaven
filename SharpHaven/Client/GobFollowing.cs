using System.Drawing;

namespace SharpHaven.Client
{
	public class GobFollowing
	{
		public GobFollowing(Gob gob, Point offset, byte szo)
		{
			Gob = gob;
			Offset = offset;
			Szo = szo;
		}

		public Gob Gob { get; }

		public Point Offset { get; }

		public byte Szo { get; }
	}
}
