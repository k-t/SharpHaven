using MonoHaven.Graphics;

namespace MonoHaven
{
	public class MapTile
	{
		private readonly byte type;
		private readonly Texture texture;

		public MapTile(byte type, Texture texture)
		{
			this.type = type;
			this.texture = texture;
		}

		public byte Type
		{
			get { return type; }
		}

		public Texture Texture
		{
			get { return texture; }
		}

		public Texture[] Transitions
		{
			get;
			set;
		}
	}
}
