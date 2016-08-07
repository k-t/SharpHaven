using System.Text;
using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class TooltipLayerHandler : GenericLayerHandler<TooltipLayer>
	{
		public TooltipLayerHandler() : base("tooltip")
		{
		}

		protected override TooltipLayer Deserialize(ByteBuffer buffer)
		{
			var text = Encoding.UTF8.GetString(buffer.ReadRemaining());
			return new TooltipLayer { Text = text };
		}

		protected override void Serialize(ByteBuffer writer, TooltipLayer tooltip)
		{
			writer.Write(Encoding.UTF8.GetBytes(tooltip.Text));
		}
	}
}