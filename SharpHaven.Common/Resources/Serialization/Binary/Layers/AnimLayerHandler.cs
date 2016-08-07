using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class AnimLayerHandler : GenericLayerHandler<AnimLayer>
	{
		public AnimLayerHandler() : base("anim")
		{
		}

		protected override AnimLayer Deserialize(ByteBuffer buffer)
		{
			var anim = new AnimLayer();
			anim.Id = buffer.ReadInt16();
			anim.Duration = buffer.ReadUInt16();
			var frameCount = buffer.ReadUInt16();
			anim.Frames = new short[frameCount];
			for (int i = 0; i < frameCount; i++)
				anim.Frames[i] = buffer.ReadInt16();
			return anim;
		}

		protected override void Serialize(ByteBuffer writer, AnimLayer anim)
		{
			writer.Write(anim.Id);
			writer.Write(anim.Duration);
			writer.Write((ushort)anim.Frames.Length);
			foreach (var frame in anim.Frames)
				writer.Write(frame);
		}
	}
}