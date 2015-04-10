using System;
using System.IO;

namespace SharpHaven.Resources.Serialization.Binary
{
	internal class ImageDataSerializer : IBinaryDataLayerSerializer
	{
		public string LayerName
		{
			get { return "image"; }
		}

		public Type LayerType
		{
			get { return typeof(ImageData); }
		}

		public object Deserialize(BinaryReader reader, int size)
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