using System.Linq;
using SharpHaven.Graphics;
using SharpHaven.Utils;

namespace SharpHaven.Resources.Serialization.Binary.Layers
{
	internal class NegLayerHandler : GenericLayerHandler<NegLayer>
	{
		private const int DirectionCount = 8;

		public NegLayerHandler() : base("neg")
		{
		}

		protected override NegLayer Deserialize(ByteBuffer buffer)
		{
			var neg = new NegLayer();
			neg.Center = buffer.ReadInt16Coord();
			neg.Hitbox = Rect.FromLTRB(buffer.ReadInt16Coord(), buffer.ReadInt16Coord());
			// not sure what that data means but preserve it anyway
			neg.Sz = buffer.ReadInt16Coord();
			var en = buffer.ReadByte(); /* number of E? */
			neg.Ep = new Coord2D[DirectionCount][];
			for (int i = 0; i < en; i++)
			{
				var epid = buffer.ReadByte();
				var cnt = buffer.ReadUInt16();
				neg.Ep[epid] = new Coord2D[cnt];
				for (int j = 0; j < cnt; j++)
					neg.Ep[epid][j] = buffer.ReadInt16Coord();
			}
			return neg;
		}

		protected override void Serialize(ByteBuffer writer, NegLayer neg)
		{
			writer.WriteInt16Coord(neg.Center);
			writer.WriteInt16Coord(neg.Hitbox.Left, neg.Hitbox.Top);
			writer.WriteInt16Coord(neg.Hitbox.Right, neg.Hitbox.Bottom);
			writer.WriteInt16Coord(neg.Sz);
			writer.Write((byte)neg.Ep.Count(x => x != null));
			for (int i = 0; i < neg.Ep.Length; i++)
			{
				if (neg.Ep[i] == null)
					continue;
				writer.Write((byte)i);
				writer.Write((ushort)neg.Ep[i].Length);
				for (int j = 0; j < neg.Ep[i].Length; j++)
					writer.WriteInt16Coord(neg.Ep[i][j]);
			}
		}
	}
}