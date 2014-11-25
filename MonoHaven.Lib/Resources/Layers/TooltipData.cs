using System.IO;
using System.Text;

namespace MonoHaven.Resources
{
	public class TooltipData
	{
		public string Text { get; set; }
	}

	public class TooltipSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "tooltip"; }
		}

		public object Deserialize(int size, BinaryReader reader)
		{
			var text = Encoding.UTF8.GetString(reader.ReadBytes(size));
			return new TooltipData { Text = text };
		}
	}
}
