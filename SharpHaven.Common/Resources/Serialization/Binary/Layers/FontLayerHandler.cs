using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class FontLayerHandler : GenericLayerHandler<FontLayer>
	{
		private const byte Version = 1;

		public FontLayerHandler() : base("font")
		{
		}

		protected override FontLayer Deserialize(ByteBuffer buffer)
		{
			var version = buffer.ReadByte();
			if (version != 1)
				throw new ResourceException($"Unknown font layer version: {version}");

			var data = new FontLayer();
			data.Type = buffer.ReadByte();
			switch (data.Type)
			{
				case 0:
					data.Bytes = buffer.ReadRemaining();
					break;
				default:
					throw new ResourceException($"Unknown font type: {data.Type}");
			}
			return data;
		}

		protected override void Serialize(ByteBuffer writer, FontLayer data)
		{
			writer.Write(Version);
			writer.Write(data.Type);
			writer.Write(data.Bytes);
		}
	}
}