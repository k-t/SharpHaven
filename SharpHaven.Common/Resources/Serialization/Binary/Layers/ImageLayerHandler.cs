using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class ImageLayerHandler : GenericLayerHandler<ImageLayer>
	{
		public ImageLayerHandler() : base("image")
		{
		}

		protected override ImageLayer Deserialize(ByteBuffer buffer)
		{
			var img = new ImageLayer();
			img.Z = buffer.ReadInt16();
			img.SubZ = buffer.ReadInt16();
			/* Obsolete flag 1: Layered */
			img.IsLayered = buffer.ReadBoolean();
			img.Id = buffer.ReadInt16();
			img.Offset = buffer.ReadInt16Coord();
			img.Data = buffer.ReadRemaining();
			return img;
		}

		protected override void Serialize(ByteBuffer writer, ImageLayer img)
		{
			writer.Write(img.Z);
			writer.Write(img.SubZ);
			writer.Write(img.IsLayered);
			writer.Write(img.Id);
			writer.WriteInt16Coord(img.Offset);
			writer.Write(img.Data);
		}
	}
}