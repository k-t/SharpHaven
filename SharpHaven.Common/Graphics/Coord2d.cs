namespace SharpHaven.Graphics
{
	public struct Coord2d
	{
		public static readonly Coord2d Empty = new Coord2d(0, 0);

		public Coord2d(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X { get; set; }

		public int Y { get; set; }

		public static bool operator ==(Coord2d a, Coord2d b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Coord2d a, Coord2d b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			return (obj is Coord2d) && Equals((Coord2d)obj);
		}

		public bool Equals(Coord2d other)
		{
			return X == other.X && Y == other.Y;
		}

		public override int GetHashCode()
		{
			return X ^ Y;
		}
	}
}
