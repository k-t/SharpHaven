using System;
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
		public bool IsLayered { get; set; }
	}

	public class ImageDataSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "image"; }
		}

		public Type LayerType
		{
			get { return typeof(ImageData); }
		}

		public object Deserialize(int size, BinaryReader reader)
		{
			var img = new ImageData();
			img.Z = reader.ReadInt16();
			img.SubZ = reader.ReadInt16();
			/* Obsolete flag 1: Layered */
			img.IsLayered = reader.ReadBoolean();
			img.Id = reader.ReadInt16();
			img.Offset = reader.ReadPoint();
			img.Data = new byte[size - 11];
			reader.Read(img.Data, 0, img.Data.Length);
			return img;
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var img = (ImageData)data;
			writer.Write(img.Z);
			writer.Write(img.SubZ);
			writer.Write(img.IsLayered);
			writer.Write(img.Id);
			writer.WritePoint(img.Offset);
			writer.Write(img.Data);
		}
	}
}

