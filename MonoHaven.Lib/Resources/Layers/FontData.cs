using System.IO;

namespace MonoHaven.Resources
{
	public class FontData
	{
		public byte[] Data { get; set; }
	}

	public class FontDataSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "font"; }
		}

		public object Deserialize(int size, BinaryReader reader)
		{
			var data = reader.ReadBytes(size);
			return new FontData { Data = data };
		}
	}
}
