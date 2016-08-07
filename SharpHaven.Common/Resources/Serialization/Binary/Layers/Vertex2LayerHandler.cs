using System;
using System.Collections.Generic;
using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	public class Vertex2LayerHandler : GenericLayerHandler<VertexLayer>
	{
		public Vertex2LayerHandler() : base("vbuf2")
		{
		}

		protected override VertexLayer Deserialize(ByteBuffer buffer)
		{
			var layer = new VertexLayer();
			layer.Flags = buffer.ReadByte();
			layer.VertexCount = buffer.ReadUInt16();
			while (buffer.HasRemaining)
				ReadArray(layer, buffer);
			return layer;
		}

		protected override void Serialize(ByteBuffer buffer, VertexLayer layer)
		{
			buffer.Write(layer.Flags);
			buffer.Write(layer.VertexCount);
			WriteArray(buffer, layer.Positions, "pos", WriteFloat);
			WriteArray(buffer, layer.Normals, "nrm", WriteFloat);
			WriteArray(buffer, layer.Tangents, "tan", WriteFloat);
			WriteArray(buffer, layer.Bitangents, "bit", WriteFloat);
			WriteArray(buffer, layer.Colors, "col", WriteFloat);
			WriteArray(buffer, layer.TexCoords, "tex", WriteFloat);

			if (layer.Bones != null)
			{
				buffer.WriteCString("bones");
				buffer.Write(layer.Bones.Mba);
				foreach (var bone in layer.Bones.Bones)
					WriteBone(buffer, bone);
				// end of sequence
				buffer.Write((ushort)0);
				buffer.Write((ushort)0);
			}
		}

		private static void ReadArray(VertexLayer layer, ByteBuffer buffer)
		{
			var vertexCount = layer.VertexCount;
			var type = buffer.ReadCString();
			switch (type)
			{
				case "pos":
					layer.Positions = ReadArray(ReadFloat, buffer, vertexCount * 3);
					break;
				case "nrm":
					layer.Normals = ReadArray(ReadFloat, buffer, vertexCount * 3);
					break;
				case "tan":
					layer.Tangents = ReadArray(ReadFloat, buffer, vertexCount * 3);
					break;
				case "bit":
					layer.Bitangents = ReadArray(ReadFloat, buffer, vertexCount * 3);
					break;
				case "col":
					layer.Colors = ReadArray(ReadFloat, buffer, vertexCount * 4);
					break;
				case "tex":
					layer.TexCoords = ReadArray(ReadFloat, buffer, vertexCount * 2);
					break;
				case "bones":
					layer.Bones = new VertexLayer.BoneArray();
					layer.Bones.Mba = buffer.ReadByte();
					var bones = new List<VertexLayer.Bone>();
					while (true)
					{
						var bone = ReadBone(buffer);
						if (bone == null)
							break;
						bones.Add(bone);
					}
					layer.Bones.Bones = bones.ToArray();
					break;
				default:
					throw new ResourceException("Unsupported vertex attrib array type: " + type);
			}
		}

		private static T[] ReadArray<T>(Func<ByteBuffer, T> itemReader, ByteBuffer buffer, int itemCount)
		{
			var array = new T[itemCount];
			for (int i = 0; i < array.Length; i++)
				array[i] = itemReader(buffer);
			return array;
		}

		private static float ReadFloat(ByteBuffer buffer)
		{
			return buffer.ReadSingle();
		}

		private static VertexLayer.Bone ReadBone(ByteBuffer buffer)
		{
			var boneName = buffer.ReadCString();
			if (string.IsNullOrEmpty(boneName))
				return null;
			var vertices = new List<VertexLayer.BoneVertex>();
			while (true)
			{
				var run = buffer.ReadUInt16();
				var vn = buffer.ReadUInt16();
				if (run == 0)
					break;
				var weights = new float[run];
				for (int i = 0; i < run; i++)
					weights[i] = buffer.ReadSingle();
				vertices.Add(new VertexLayer.BoneVertex { Vn = vn, Weights = weights });
			}
			return new VertexLayer.Bone {Name = boneName, Vertices = vertices.ToArray()};
		}

		private static void WriteArray<T>(ByteBuffer buffer, T[] array, string type, Action<ByteBuffer, T> itemWriter)
		{
			if (array == null)
				return;
			buffer.WriteCString(type);
			for (int i = 0; i < array.Length; i++)
				itemWriter(buffer, array[i]);
		}

		private static void WriteFloat(ByteBuffer buffer, float value)
		{
			buffer.Write(value);
		}

		private static void WriteBone(ByteBuffer buffer, VertexLayer.Bone bone)
		{
			buffer.WriteCString(bone.Name);
			foreach (var v in bone.Vertices)
			{
				buffer.Write((ushort)v.Weights.Length);
				buffer.Write(v.Vn);
				for (int i = 0; i < v.Weights.Length; i++)
					buffer.Write(v.Weights[i]);
			}
		}
	}
}
