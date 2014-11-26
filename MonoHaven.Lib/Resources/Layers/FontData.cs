using System;
using System.IO;

namespace MonoHaven.Resources
{
	public class FontData
	{
		public byte[] Data { get; set; }
	}

	public class FontDataSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "font"; }
		}

		public Type LayerType
		{
			get { return typeof(FontData); }
		}

		public object Deserialize(BinaryReader reader, int size)
		{
			var data = reader.ReadBytes(size);
			return new FontData { Data = data };
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var font = (FontData)data;
			writer.Write(font.Data);
		}
	}
}
