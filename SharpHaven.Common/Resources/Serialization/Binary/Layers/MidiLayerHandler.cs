using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class MidiLayerHandler : GenericLayerHandler<MidiLayer>
	{
		public MidiLayerHandler() : base("midi")
		{
		}

		protected override MidiLayer Deserialize(ByteBuffer buffer)
		{
			var bytes = buffer.ReadRemaining();
			return new MidiLayer { Bytes = bytes };
		}

		protected override void Serialize(ByteBuffer writer, MidiLayer midi)
		{
			writer.Write(midi.Bytes);
		}
	}
}
