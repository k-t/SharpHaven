using System.Drawing;

namespace MonoHaven.Graphics.Sprites
{
	public class SpritePart
	{
		private readonly int id;
		private readonly Drawable image;
		private Rectangle bounds;
		private readonly int z;
		private readonly int subz;

		public SpritePart(Drawable image, Point offset, int z, int subz)
			: this(-1, image, offset, z, subz)
		{
		}

		public SpritePart(int id, Drawable image, Point offset, int z, int subz)
		{
			this.id = id;
			this.image = image;
			this.bounds = new Rectangle(offset.X, offset.Y, image.Width, image.Height);
			this.z = z;
			this.subz = subz;
		}

		public int Id
		{
			get { return id; }
		}

		public Drawable Image
		{
			get { return image; }
		}

		public Point Offset
		{
			get { return bounds.Location; }
			set { bounds.Location = value; }
		}

		public int Width
		{
			get { return image.Width; }
		}

		public int Height
		{
			get { return image.Height; }
		}

		public int Z
		{
			get { return z; }
		}

		public int SubZ
		{
			get { return subz; }
		}

		public bool CheckHit(int x, int y)
		{
			return image.CheckHit(x - Offset.X, y - Offset.Y);
		}
	}
}
