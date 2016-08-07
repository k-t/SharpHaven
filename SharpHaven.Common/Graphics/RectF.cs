namespace SharpHaven.Graphics
{
	public struct RectF
	{
		public RectF(float x, float y, float width, float height) : this()
		{
			Location = new Coord2f(x, y);
			Size = new Coord2f(width, height);
		}

		public Coord2f Location { get; set; }

		public Coord2f Size { get; set; }

		public float X
		{
			get { return Location.X; }
		}

		public float Y
		{
			get { return Location.Y; }
		}

		public float Width
		{
			get { return Size.X; }
		}

		public float Height
		{
			get { return Size.Y; }
		}

		public float Top
		{
			get { return Y; }
		}

		public float Bottom
		{
			get { return Y + Height; }
		}

		public float Left
		{
			get { return X; }
		}

		public float Right
		{
			get { return X + Width; }
		}

		public bool Contains(int x, int y)
		{
			return (x >= Left && x <= Right) && (y >= Top && y <= Bottom);
		}

		public static RectF FromLTRB(Coord2f topLeft, Coord2f bottomRight)
		{
			return FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
		}

		public static RectF FromLTRB(float left, float top, float right, float bottom)
		{
			// TODO: check top > bottom and left > right
			return new RectF(left, top, right - left, bottom - top);
		}

		public static implicit operator RectF(Rect rect)
		{
			return new RectF(rect.X, rect.Y, rect.Width, rect.Height);
		}
	}
}
