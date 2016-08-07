using System;
using SharpHaven.Utils;

namespace SharpHaven.Graphics
{
	public struct Rect
	{
		public static readonly Rect Empty = new Rect();

		public Rect(int x, int y, int width, int height) : this()
		{
			Location = new Coord2d(x, y);
			Size = new Coord2d(width, height);
		}

		public Coord2d Location { get; set; }

		public Coord2d Size { get; set; }

		public int X
		{
			get { return Location.X; }
		}

		public int Y
		{
			get { return Location.Y; }
		}

		public int Width
		{
			get { return Size.X; }
		}

		public int Height
		{
			get { return Size.Y; }
		}

		public int Top
		{
			get { return Y; }
		}

		public int Bottom
		{
			get { return Y + Height; }
		}

		public int Left
		{
			get { return X; }
		}

		public int Right
		{
			get { return X + Width; }
		}

		public static Rect FromLTRB(Coord2d topLeft, Coord2d bottomRight)
		{
			return FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
		}

		public static Rect FromLTRB(int left, int top, int right, int bottom)
		{
			// TODO: check top > bottom and left > right
			return new Rect(left, top, right - left, bottom - top);
		}

		public static bool operator ==(Rect a, Rect b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Rect a, Rect b)
		{
			return !(a == b);
		}

		public bool Contains(Coord2d c)
		{
			return Contains(c.X, c.Y);
		}

		public bool Contains(int x, int y)
		{
			return (x >= Left && x <= Right) && (y >= Top && y <= Bottom);
		}

		public override bool Equals(object obj)
		{
			return (obj is Rect) && Equals((Rect)obj);
		}

		public bool Equals(Rect other)
		{
			return Location == other.Location && Size == other.Size;
		}

		public override int GetHashCode()
		{
			return Location.GetHashCode() ^ Size.GetHashCode();
		}

		public void Offset(int x, int y)
		{
			Location = Location.Add(x, y);
		}
	}
}
