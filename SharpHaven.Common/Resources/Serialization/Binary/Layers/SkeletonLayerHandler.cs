using System.Collections.Generic;
using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class SkeletonLayerHandler : GenericLayerHandler<SkeletonLayer>
	{
		public SkeletonLayerHandler() : base("skel")
		{
		}

		protected override SkeletonLayer Deserialize(ByteBuffer buffer)
		{
			var bones = new List<SkeletonBone>();
			while (buffer.HasRemaining)
			{
				var bone = new SkeletonBone();
				bone.Name = buffer.ReadCString();
				bone.Position = new float[3];
				for (int i = 0; i < 3; i++)
					bone.Position[i] = (float)buffer.ReadFloat40();
				bone.RotationAxis = new float[3];
				for (int i = 0; i < 3; i++)
					bone.RotationAxis[i] = (float)buffer.ReadFloat40();
				bone.RotationAngle = (float)buffer.ReadFloat40();
				bone.Parent = buffer.ReadCString();
				bones.Add(bone);
			}
			return new SkeletonLayer {Bones = bones.ToArray()};
		}

		protected override void Serialize(ByteBuffer writer, SkeletonLayer layer)
		{
			foreach (var bone in layer.Bones)
			{
				writer.WriteCString(bone.Name);
				foreach (var n in bone.Position)
					writer.WriteFloat40(n);
				foreach (var n in bone.RotationAxis)
					writer.WriteFloat40(n);
				writer.WriteFloat40(bone.RotationAngle);
				writer.WriteCString(bone.Parent ?? "");
			}
		}
	}
}
