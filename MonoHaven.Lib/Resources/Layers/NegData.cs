using System.Drawing;
using System.IO;

namespace MonoHaven.Resources
{
	public class NegData
	{
		public Point Center { get; set; }
	}

	public class NegDataSerializer : IDataLayerSerializer
	{
		public string LayerName
		{
			get { return "neg"; }
		}

		public object Deserialize(int size, BinaryReader reader)
		{
			var neg = new NegData();
			neg.Center = reader.ReadPoint();
			reader.ReadPoint(); /* bc */
			reader.ReadPoint(); /* bs */
			reader.ReadPoint(); /* sz */
			var en = reader.ReadByte();
			for (int i = 0; i < en; i++)
			{
				reader.ReadByte(); /* epid */
				var cnt = reader.ReadUInt16();
				for (int j = 0; j < cnt; j++)
					reader.ReadPoint(); /* ep[epid][j] */
			}
			return neg;
		}
	}
}
