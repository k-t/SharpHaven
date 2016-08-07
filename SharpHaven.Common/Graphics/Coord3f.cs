namespace SharpHaven.Graphics
{
	public struct Coord3f
	{
		public static readonly Coord3f O = new Coord3f(0, 0, 0);

		public Coord3f(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public float X { get; set; }

		public float Y { get; set; }

		public float Z { get; set; }

		public override bool Equals(object obj)
		{
			return (obj is Coord3f) && Equals((Coord3f)obj);
		}

		public bool Equals(Coord3f other)
		{
			return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
		}

		public override int GetHashCode()
		{
			var hc = X.GetHashCode();
			hc = (hc * 397) ^ Y.GetHashCode();
			hc = (hc * 397) ^ Z.GetHashCode();
			return hc;
		}
	}
}
