using System;
using System.IO;

namespace MonoHaven.Resources.Serialization.Binary
{
	public class CodeDataSerializer : IBinaryDataLayerSerializer
	{
		public string LayerName
		{
			get { return "code"; }
		}

		public Type LayerType
		{
			get { return typeof(CodeData); }
		}

		public object Deserialize(BinaryReader reader, int size)
		{
			var code = new CodeData();
			code.Name = reader.ReadCString();
			code.ByteCode = reader.ReadBytes(size - code.Name.Length - 1);
			return code;
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var code = (CodeData)data;
			writer.WriteCString(code.Name);
			writer.Write(code.ByteCode);
		}
	}
}
