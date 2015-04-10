using System;
using System.IO;
using System.Text;

namespace SharpHaven.Resources.Serialization.Binary
{
	internal class TooltipDataSerializer : IBinaryDataLayerSerializer
	{
		public string LayerName
		{
			get { return "tooltip"; }
		}

		public Type LayerType
		{
			get { return typeof(TooltipData); }
		}

		public object Deserialize(BinaryReader reader, int size)
		{
			var text = Encoding.UTF8.GetString(reader.ReadBytes(size));
			return new TooltipData { Text = text };
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var tooltip = (TooltipData)data;
			writer.Write(Encoding.UTF8.GetBytes(tooltip.Text));
		}
	}
}