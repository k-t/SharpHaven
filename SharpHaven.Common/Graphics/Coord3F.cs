using System;

namespace SharpHaven.Graphics
{
	public struct Coord3F : IEquatable<Coord3F>
	{
		public static readonly Coord3F Empty = new Coord3F(0, 0, 0);

		public Coord3F(float x, float y, float z)
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
			return (obj is Coord3F) && Equals((Coord3F)obj);
		}

		public bool Equals(Coord3F other)
		{
			return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
		}

		public override int GetHashCode()
		{
			int hash = 13;
			hash = (hash * 7) + X.GetHashCode();
			hash = (hash * 7) + Y.GetHashCode();
			hash = (hash * 7) + Z.GetHashCode();
			return hash;
		}

		public static bool operator ==(Coord3F a, Coord3F b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Coord3F a, Coord3F b)
		{
			return !(a == b);
		}
	}
}
