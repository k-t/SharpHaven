using System;
using SharpHaven.Graphics;
using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class TexLayerHandler : GenericLayerHandler<TexLayer>
	{
		private const byte ImagePart = 0;
		private const byte MipmapPart = 1;
		private const byte MagFilterPart = 2;
		private const byte MinFilterPart = 3;
		private const byte MaskPart = 4;

		private static readonly TexMipmap[] Mipmaps = {
			TexMipmap.Average,
			TexMipmap.Average,
			TexMipmap.Random,
			TexMipmap.Cnt,
			TexMipmap.Dav,
		};

		private static readonly TexMagFilter[] MagFilters = {
			TexMagFilter.Nearest,
			TexMagFilter.Linear,
		};

		private static readonly TexMinFilter[] MinFilters = {
			TexMinFilter.Nearest,
			TexMinFilter.Linear,
			TexMinFilter.NearestMipmapNearest,
			TexMinFilter.NearestMipmapLinear,
			TexMinFilter.LinearMipmapNearest,
			TexMinFilter.LinearMipmapLinear,
		};

		public TexLayerHandler() : base("tex")
		{
		}

		protected override TexLayer Deserialize(ByteBuffer buffer)
		{
			var data = new TexLayer();
			data.Id = buffer.ReadInt16();
			data.Offset = new Coord2D(buffer.ReadUInt16(), buffer.ReadUInt16());
			data.Size = new Coord2D(buffer.ReadUInt16(), buffer.ReadUInt16());
			data.Mipmap = TexMipmap.None;
			TexMinFilter? minFilter = null;
			TexMagFilter? magFilter = null;
			while (buffer.HasRemaining)
			{
				int part = buffer.ReadByte();
				switch (part)
				{
					case ImagePart:
						data.Image = buffer.ReadBytes(buffer.ReadInt32());
						break;
					case MipmapPart:
						data.Mipmap = Mipmaps[buffer.ReadByte()];
						break;
					case MagFilterPart:
						magFilter = MagFilters[buffer.ReadByte()];
						break;
					case MinFilterPart:
						minFilter = MinFilters[buffer.ReadByte()];
						break;
					case MaskPart:
						data.Mask = buffer.ReadBytes(buffer.ReadInt32());
						break;
					default:
						throw new ResourceException($"Unknown texture data part: {part}");
				}
				
			}
			data.MagFilter = magFilter ?? TexMagFilter.Linear;
			data.MinFilter = minFilter ?? (
				data.Mipmap == TexMipmap.None
				? TexMinFilter.Linear
				: TexMinFilter.LinearMipmapLinear);
			return data;
		}

		protected override void Serialize(ByteBuffer writer, TexLayer data)
		{
			writer.Write(data.Id);
			writer.Write((ushort)data.Offset.X);
			writer.Write((ushort)data.Offset.Y);
			writer.Write((ushort)data.Size.X);
			writer.Write((ushort)data.Size.Y);
			// image
			if (data.Image != null)
			{
				writer.Write(ImagePart);
				writer.Write(data.Image.Length);
				writer.Write(data.Image);
			}
			// mipmap
			if (data.Mipmap != TexMipmap.None)
			{
				writer.Write(MipmapPart);
				writer.Write((byte)Array.IndexOf(Mipmaps, data.Mipmap));
			}
			// magfilter
			writer.Write(MagFilterPart);
			writer.Write((byte)Array.IndexOf(MagFilters, data.MagFilter));
			// minfilter
			writer.Write(MinFilterPart);
			writer.Write((byte)Array.IndexOf(MinFilters, data.MinFilter));
			// mask
			if (data.Mask != null)
			{
				writer.Write(MaskPart);
				writer.Write(data.Mask.Length);
				writer.Write(data.Mask);
			}
		}
	}
}
