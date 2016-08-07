using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class FoodEventLayerHandler : GenericLayerHandler<FoodEventLayer>
	{
		private const byte Version = 1;

		public FoodEventLayerHandler() : base("foodev")
		{
		}

		protected override FoodEventLayer Deserialize(ByteBuffer buffer)
		{
			var version = buffer.ReadByte();
			if (version != 1)
				throw new ResourceException($"Unknown foodev version: {version}");

			var data = new FoodEventLayer();
			data.Color = buffer.ReadColor();
			data.Name = buffer.ReadCString();
			data.Sort = buffer.ReadInt16();
			return data;
		}

		protected override void Serialize(ByteBuffer writer, FoodEventLayer data)
		{
			writer.Write(Version);
			writer.WriteColor(data.Color);
			writer.WriteCString(data.Name);
			writer.Write(data.Sort);
		}
	}
}
