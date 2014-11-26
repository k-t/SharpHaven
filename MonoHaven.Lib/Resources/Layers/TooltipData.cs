using System;
using System.IO;
using System.Text;

namespace MonoHaven.Resources
{
	public class TooltipData
	{
		public string Text { get; set; }
	}

	public class TooltipDataSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "tooltip"; }
		}

		public Type LayerType
		{
			get { return typeof(TooltipData); }
		}

		public object Deserialize(int size, BinaryReader reader)
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
