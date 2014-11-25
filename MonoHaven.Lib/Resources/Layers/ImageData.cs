using System.Drawing;
using System.IO;

namespace MonoHaven.Resources
{
	public class ImageData
	{
		public short Id { get; set; }
		public short Z { get; set; }
		public short SubZ { get; set; }
		public byte[] Data { get; set; }
		public Point Offset { get; set; }
	}

	public class ImageDataSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "image"; }
		}

		public object Deserialize(int size, BinaryReader reader)
		{
			var img = new ImageData();
			img.Z = reader.ReadInt16();
			img.SubZ = reader.ReadInt16();
			/* Obsolete flag 1: Layered */
			reader.ReadByte();
			img.Id = reader.ReadInt16();
			img.Offset = reader.ReadPoint();
			img.Data = new byte[size - 11];
			reader.Read(img.Data, 0, img.Data.Length);
			return img;
		}
	}
}

