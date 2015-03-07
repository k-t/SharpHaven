using System;
using System.IO;
using System.Text;

namespace MonoHaven.Resources.Serialization.Binary
{
	internal class TextDataSerializer : IBinaryDataLayerSerializer
	{
		public string LayerName
		{
			get { return "pagina"; }
		}

		public Type LayerType
		{
			get { return typeof(TextData); }
		}

		public object Deserialize(BinaryReader reader, int size)
		{
			var text = Encoding.UTF8.GetString(reader.ReadBytes(size));
			return new TextData { Text =  text };
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var text = (TextData)data;
			writer.Write(Encoding.UTF8.GetBytes(text.Text));
		}
	}
}