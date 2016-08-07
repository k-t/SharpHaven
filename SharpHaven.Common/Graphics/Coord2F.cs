using System;

namespace SharpHaven.Graphics
{
	public struct Coord2F : IEquatable<Coord2F>
	{
		public static readonly Coord2F Empty = new Coord2F(0, 0);

		public Coord2F(float x, float y)
		{
			X = x;
			Y = y;
		}

		public Coord2F(Coord2F other)
		{
			X = other.X;
			Y = other.Y;
		}

		public float X { get; set; }

		public float Y { get; set; }

		public Coord2F Add(float x, float y)
		{
			return new Coord2F(X + x, Y + y);
		}

		public Coord2F Add(Coord2F b)
		{
			return Add(b.X, b.Y);
		}

		public Coord2F Add(float value)
		{
			return Add(value, value);
		}

		public Coord2F Sub(float x, float y)
		{
			return new Coord2F(X - x, Y - y);
		}

		public Coord2F Sub(Coord2F b)
		{
			return Sub(b.X, b.Y);
		}

		public Coord2F Sub(float value)
		{
			return Sub(-value, -value);
		}

		public Coord2F Mul(float x, float y)
		{
			return new Coord2F(X * x, Y * y);
		}

		public Coord2F Mul(Coord2F b)
		{
			return Mul(b.X, b.Y);
		}

		public Coord2F Mul(float value)
		{
			return Mul(value, value);
		}

		public double DistanceTo(Coord2F other)
		{
			return Math.Sqrt(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2));
		}

		public bool Equals(Coord2F other)
		{
			return X == other.X && Y == other.Y;
		}

		public override bool Equals(object obj)
		{
			return (obj is Coord2F) && Equals((Coord2F)obj);
		}

		public override int GetHashCode()
		{
			int hash = 13;
			hash = (hash * 7) + X.GetHashCode();
			hash = (hash * 7) + Y.GetHashCode();
			return hash;
		}

		public static Coord2F operator +(Coord2F a, Coord2F b)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));
			if (b == null)
				throw new ArgumentNullException(nameof(b));

			return a.Add(b.X, b.Y);
		}

		public static Coord2F operator +(Coord2F a, float value)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));

			return a.Add(value);
		}

		public static Coord2F operator -(Coord2F a, Coord2F b)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));
			if (b == null)
				throw new ArgumentNullException(nameof(b));

			return a.Sub(b.X, b.Y);
		}

		public static Coord2F operator -(Coord2F a, float value)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));

			return a.Sub(value);
		}

		public static Coord2F operator *(Coord2F a, Coord2F b)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));
			if (b == null)
				throw new ArgumentNullException(nameof(b));

			return a.Mul(b.X, b.Y);
		}

		public static Coord2F operator *(Coord2F a, float value)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));

			return a.Mul(value);
		}

		public static bool operator ==(Coord2F a, Coord2F b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Coord2F a, Coord2F b)
		{
			return !(a == b);
		}
	}
}
