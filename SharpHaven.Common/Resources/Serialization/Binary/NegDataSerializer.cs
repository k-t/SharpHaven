using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SharpHaven.Resources.Serialization.Binary
{
	internal class NegDataSerializer : IBinaryDataLayerSerializer
	{
		private const int DirectionCount = 8;

		public string LayerName
		{
			get { return "neg"; }
		}

		public Type LayerType
		{
			get { return typeof(NegData); }
		}

		public object Deserialize(BinaryReader reader, int size)
		{
			var neg = new NegData();
			neg.Center = reader.ReadPoint();
			var hul = Geometry.ScreenToMap(reader.ReadPoint());
			var hbr = Geometry.ScreenToMap(reader.ReadPoint());
			neg.Hitbox = Rectangle.FromLTRB(hul.X, hul.Y, hbr.X, hbr.Y);
			// not sure what that data means but preserve it anyway
			neg.Sz = reader.ReadPoint();
			var en = reader.ReadByte(); /* number of E? */
			neg.Ep = new Point[DirectionCount][];
			for (int i = 0; i < en; i++)
			{
				var epid = reader.ReadByte();
				var cnt = reader.ReadUInt16();
				neg.Ep[epid] = new Point[cnt];
				for (int j = 0; j < cnt; j++)
					neg.Ep[epid][j] = reader.ReadPoint();
			}
			return neg;
		}

		public void Serialize(BinaryWriter writer, object data)
		{
			var neg = (NegData)data;
			writer.WritePoint(neg.Center);
			writer.WritePoint(Geometry.MapToScreen(neg.Hitbox.Location));
			writer.WritePoint(Geometry.MapToScreen(neg.Hitbox.Right, neg.Hitbox.Bottom));
			writer.WritePoint(neg.Sz);
			writer.Write((byte)neg.Ep.Count(x => x != null));
			for (int i = 0; i < neg.Ep.Length; i++)
			{
				if (neg.Ep[i] == null)
					continue;
				writer.Write((byte)i);
				writer.Write((ushort)neg.Ep[i].Length);
				for (int j = 0; j < neg.Ep[i].Length; j++)
					writer.WritePoint(neg.Ep[i][j]);
			}
		}
	}
}