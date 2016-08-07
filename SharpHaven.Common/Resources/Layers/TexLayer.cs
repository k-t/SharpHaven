using SharpHaven.Graphics;

namespace SharpHaven.Resources
{
	public class TexLayer
	{
		public short Id { get; set; }
		public Coord2d Offset { get; set; }
		public Coord2d Size { get; set; }
		public byte[] Image { get; set; }
		public byte[] Mask { get; set; }
		public TexMipmap Mipmap { get; set; }
		public TexMagFilter MagFilter { get; set; }
		public TexMinFilter MinFilter { get; set; }
	}

	public enum TexMipmap
	{
		None,
		Average,
		Random,
		Cnt, // ??
		Dav, // ??
	}

	public enum TexMagFilter
	{
		Nearest,
		Linear,
	}

	public enum TexMinFilter
	{
		Nearest,
		Linear,
		NearestMipmapNearest,
		NearestMipmapLinear,
		LinearMipmapNearest,
		LinearMipmapLinear,
	}
}
