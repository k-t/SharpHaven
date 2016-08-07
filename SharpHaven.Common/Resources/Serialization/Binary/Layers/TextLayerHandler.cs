using System.Text;
using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class TextLayerHandler : GenericLayerHandler<TextLayer>
	{
		public TextLayerHandler() : base("pagina")
		{
		}

		protected override TextLayer Deserialize(ByteBuffer buffer)
		{
			var text = Encoding.UTF8.GetString(buffer.ReadRemaining());
			return new TextLayer { Text =  text };
		}

		protected override void Serialize(ByteBuffer writer, TextLayer text)
		{
			writer.Write(Encoding.UTF8.GetBytes(text.Text));
		}
	}
}