using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class Tileset2LayerHandler : GenericLayerHandler<Tileset2Layer>
	{
		private const byte Tiler = 0;
		private const byte Flavor = 1;
		private const byte Tags = 2;

		public Tileset2LayerHandler() : base("tileset2")
		{
		}

		protected override Tileset2Layer Deserialize(ByteBuffer buffer)
		{
			var data = new Tileset2Layer();
			while (buffer.HasRemaining)
			{
				var part = buffer.ReadByte();
				switch (part)
				{
					case Tiler:
						data.TilerName = buffer.ReadCString();
						data.TilerAttributes = buffer.ReadList();
						break;
					case Flavor:
						var flavorCount = buffer.ReadUInt16();
						data.FlavorDensity = buffer.ReadUInt16();
						data.FlavorObjects = new FlavorObjectData[flavorCount];
						for (int i = 0; i < flavorCount; i++)
						{
							var fob = new FlavorObjectData();
							fob.ResName = buffer.ReadCString();
							fob.ResVersion = buffer.ReadUInt16();
							fob.Weight = buffer.ReadByte();
							data.FlavorObjects[i] = fob;
						}
						break;
					case Tags:
						data.Tags = new string[buffer.ReadByte()];
						for (int i = 0; i < data.Tags.Length; i++)
							data.Tags[i] = buffer.ReadCString();
						break;
					default:
						throw new ResourceException($"Invalid tileset part {part}");
				}
			}
			return data;
		}

		protected override void Serialize(ByteBuffer writer, Tileset2Layer data)
		{
			// tiler
			writer.Write(Tiler);
			writer.WriteCString(data.TilerName);
			writer.WriteList(data.TilerAttributes);
			// flavor
			writer.Write(Flavor);
			writer.Write((ushort)data.FlavorObjects.Length);
			writer.Write(data.FlavorDensity);
			foreach (var fob in data.FlavorObjects)
			{
				writer.WriteCString(fob.ResName);
				writer.Write(fob.ResVersion);
				writer.Write(fob.Weight);
			}
			// tags
			writer.Write(Tags);
			writer.Write((byte)data.Tags.Length);
			foreach (var tag in data.Tags)
				writer.WriteCString(tag);
		}
	}
}
