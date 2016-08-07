using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class NinepatchLayerHandler : GenericLayerHandler<NinepatchLayer>
	{
		public NinepatchLayerHandler() : base("ninepatch")
		{
		}

		protected override NinepatchLayer Deserialize(ByteBuffer buffer)
		{
			return new NinepatchLayer {
				Top = buffer.ReadByte(),
				Bottom = buffer.ReadByte(),
				Left = buffer.ReadByte(),
				Right = buffer.ReadByte()
			};
		}

		protected override void Serialize(ByteBuffer writer, NinepatchLayer ninepatch)
		{
			writer.Write(ninepatch.Top);
			writer.Write(ninepatch.Bottom);
			writer.Write(ninepatch.Left);
			writer.Write(ninepatch.Right);
		}
	}
}