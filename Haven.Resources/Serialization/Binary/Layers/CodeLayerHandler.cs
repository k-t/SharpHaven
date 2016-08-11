using Haven.Utils;

namespace Haven.Resources.Serialization.Binary.Layers
{
	internal class CodeLayerHandler : GenericLayerHandler<CodeLayer>
	{
		public CodeLayerHandler() : base("code")
		{
		}

		protected override CodeLayer Deserialize(BinaryDataReader reader)
		{
			var code = new CodeLayer();
			code.Name = reader.ReadCString();
			code.ByteCode = reader.ReadRemaining();
			return code;
		}

		protected override void Serialize(BinaryDataWriter writer, CodeLayer code)
		{
			writer.WriteCString(code.Name);
			writer.Write(code.ByteCode);
		}
	}
}
