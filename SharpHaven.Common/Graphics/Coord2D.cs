using System;

namespace SharpHaven.Graphics
{
	public struct Coord2D : IEquatable<Coord2D>
	{
		public static readonly Coord2D Empty = new Coord2D(0, 0);

		public Coord2D(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Coord2D(Coord2D other)
		{
			X = other.X;
			Y = other.Y;
		}

		public int X { get; set; }

		public int Y { get; set; }

		public Coord2D Add(int x, int y)
		{
			return new Coord2D(X + x, Y + y);
		}

		public Coord2D Add(Coord2D b)
		{
			return Add(b.X, b.Y);
		}

		public Coord2D Add(int value)
		{
			return Add(value, value);
		}

		public Coord2D Sub(int x, int y)
		{
			return new Coord2D(X - x, Y - y);
		}

		public Coord2D Sub(Coord2D b)
		{
			return Sub(b.X, b.Y);
		}

		public Coord2D Sub(int value)
		{
			return Sub(-value, -value);
		}

		public Coord2D Mul(int x, int y)
		{
			return new Coord2D(X * x, Y * y);
		}

		public Coord2D Mul(Coord2D b)
		{
			return Mul(b.X, b.Y);
		}

		public Coord2D Mul(int value)
		{
			return Mul(value, value);
		}

		public double DistanceTo(Coord2D other)
		{
			return Math.Sqrt(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2));
		}

		public bool Equals(Coord2D other)
		{
			return X == other.X && Y == other.Y;
		}

		public override bool Equals(object obj)
		{
			return (obj is Coord2D) && Equals((Coord2D)obj);
		}

		public override int GetHashCode()
		{
			int hash = 13;
			hash = (hash * 7) + X;
			hash = (hash * 7) + Y;
			return hash;
		}

		public static Coord2D operator +(Coord2D a, Coord2D b)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));
			if (b == null)
				throw new ArgumentNullException(nameof(b));

			return a.Add(b.X, b.Y);
		}

		public static Coord2D operator +(Coord2D a, int value)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));

			return a.Add(value);
		}

		public static Coord2D operator -(Coord2D a, Coord2D b)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));
			if (b == null)
				throw new ArgumentNullException(nameof(b));

			return a.Sub(b.X, b.Y);
		}

		public static Coord2D operator -(Coord2D a, int value)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));

			return a.Sub(value);
		}

		public static Coord2D operator *(Coord2D a, Coord2D b)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));
			if (b == null)
				throw new ArgumentNullException(nameof(b));

			return a.Mul(b.X, b.Y);
		}

		public static Coord2D operator *(Coord2D a, int value)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));

			return a.Mul(value);
		}

		public static bool operator ==(Coord2D a, Coord2D b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Coord2D a, Coord2D b)
		{
			return !(a == b);
		}
	}
}
