using System.Collections.Generic;
using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class Material2LayerHandler : GenericLayerHandler<MaterialLayer>
	{
		public Material2LayerHandler() : base("mat2")
		{
		}

		protected override MaterialLayer Deserialize(ByteBuffer buffer)
		{
			var layer = new MaterialLayer();
			layer.Id = buffer.ReadUInt16();
			// read materials
			var materials = new List<MaterialLayer.Material>();
			while (buffer.HasRemaining)
			{
				var mat = new MaterialLayer.Material();
				mat.Name = buffer.ReadCString();
				mat.Params = buffer.ReadList();
				materials.Add(mat);

				// WTF, loftar?
				switch (mat.Name)
				{
					case "linear":
						layer.IsLinear = true;
						break;
					case "mipmap":
						layer.IsMipmap = true;
						break;
				}
			}
			layer.Materials = materials.ToArray();
			return layer;
		}

		protected override void Serialize(ByteBuffer writer, MaterialLayer layer)
		{
			writer.Write(layer.Id);
			foreach (var mat in layer.Materials)
			{
				writer.WriteCString(mat.Name);
				writer.WriteList(mat.Params);
			}
		}
	}
}
