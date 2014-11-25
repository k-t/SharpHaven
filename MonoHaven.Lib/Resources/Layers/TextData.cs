using System.IO;
using System.Text;

namespace MonoHaven.Resources
{
	public class TextData
	{
		public string Text { get; set; }
	}

	public class TextDataSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "pagina"; }
		}

		public object Deserialize(int size, BinaryReader reader)
		{
			var text = Encoding.UTF8.GetString(reader.ReadBytes(size));
			return new TextData { Text =  text };
		}
	}
}
