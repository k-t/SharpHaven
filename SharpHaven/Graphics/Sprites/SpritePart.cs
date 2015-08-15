using System.Drawing;

namespace SharpHaven.Graphics.Sprites
{
	public class SpritePart
	{
		private Rectangle bounds;
		
		public SpritePart(Drawable image, Point offset, int z, int subz)
			: this(-1, image, offset, z, subz)
		{
		}

		public SpritePart(int id, Drawable image, Point offset, int z, int subz)
		{
			Id = id;
			Image = image;
			Z = z;
			SubZ = subz;
			bounds = new Rectangle(offset.X, offset.Y, image.Width, image.Height);
		}

		public int Id { get; }

		public Drawable Image { get; }

		public int Z { get; }

		public int SubZ { get; }

		public Point Offset
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
