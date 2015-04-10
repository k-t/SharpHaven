using System;
using System.IO;

namespace SharpHaven.Resources.Serialization.Binary
{
	internal class FontDataSerializer : IBinaryDataLayerSerializer
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