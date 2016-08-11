using Haven;

namespace SharpHaven.Client
{
	public class GobFollowing
	{
		public GobFollowing(Gob gob, Point2D offset, byte szo)
		{
			Gob = gob;
			Offset = offset;
			Szo = szo;
		}

		public Gob Gob { get; }

		public Point2D Offset { get; }

		public byte Szo { get; }
	}
}
