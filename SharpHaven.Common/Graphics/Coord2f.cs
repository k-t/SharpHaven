namespace SharpHaven.Graphics
{
	public struct Coord2f
	{
		public static readonly Coord2f Empty = new Coord2f(0, 0);

		public Coord2f(float x, float y)
		{
			X = x;
			Y = y;
		}

		public float X { get; set; }

		public float Y { get; set; }

		public override bool Equals(object obj)
		{
			return (obj is Coord3f) && Equals((Coord3f)obj);
		}

		public bool Equals(Coord3f other)
		{
			return X.Equals(other.X) && Y.Equals(other.Y);
		}

		public override int GetHashCode()
		{
			var hc = X.GetHashCode();
			hc = (hc * 397) ^ Y.GetHashCode();
			return hc;
		}
	}
}
