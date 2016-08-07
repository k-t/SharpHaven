namespace SharpHaven.Graphics.Sprites
{
	public class SpritePart
	{
		private Rect bounds;
		
		public SpritePart(Drawable image, Coord2D offset, int z, int subz)
			: this(-1, image, offset, z, subz)
		{
		}

		public SpritePart(int id, Drawable image, Coord2D offset, int z, int subz)
		{
			Id = id;
			Image = image;
			Z = z;
			SubZ = subz;
			bounds = new Rect(offset.X, offset.Y, image.Width, image.Height);
		}

		public int Id { get; }

		public Drawable Image { get; }

		public int Z { get; }

		public int SubZ { get; }

		public Coord2D Offset
		{
			get { return bounds.Location; }
			set { bounds.Location = value; }
		}

		public int Width
		{
			get { return Image.Width; }
		}

		public int Height
		{
			get { return Image.Height; }
		}

		public bool CheckHit(int x, int y)
		{
			return Image.CheckHit(x - Offset.X, y - Offset.Y);
		}
	}
}
