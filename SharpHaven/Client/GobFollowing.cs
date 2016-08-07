using SharpHaven.Graphics;

namespace SharpHaven.Client
{
	public class GobFollowing
	{
		public GobFollowing(Gob gob, Coord2D offset, byte szo)
		{
			Gob = gob;
			Offset = offset;
			Szo = szo;
		}

		public Gob Gob { get; }

		public Coord2D Offset { get; }

		public byte Szo { get; }
	}
}
