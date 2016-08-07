using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class CodeLayerHandler : GenericLayerHandler<CodeLayer>
	{
		public CodeLayerHandler() : base("code")
		{
		}

		protected override CodeLayer Deserialize(ByteBuffer buffer)
		{
			var code = new CodeLayer();
			code.Name = buffer.ReadCString();
			code.ByteCode = buffer.ReadRemaining();
			return code;
		}

		protected override void Serialize(ByteBuffer writer, CodeLayer code)
		{
			writer.WriteCString(code.Name);
			writer.Write(code.ByteCode);
		}
	}
}
