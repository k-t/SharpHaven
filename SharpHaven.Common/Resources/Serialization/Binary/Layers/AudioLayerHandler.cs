using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class AudioLayerHandler : GenericLayerHandler<AudioLayer>
	{
		public AudioLayerHandler() : base("audio")
		{
		}

		protected override AudioLayer Deserialize(ByteBuffer buffer)
		{
			return new AudioLayer { Id = "cl", Bytes = buffer.ReadRemaining() };
		}

		protected override void Serialize(ByteBuffer writer, AudioLayer audio)
		{
			writer.Write(audio.Bytes);
		}
	}
}
