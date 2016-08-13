using Haven;
using Haven.Resources;
using Haven.Utils;
using SharpHaven.Client;

namespace SharpHaven.Graphics.Sprites.Fx
{
	public class FloatTextMaker : SpriteMaker
	{
		public FloatTextMaker(Resource res)
			: base(res)
		{
		}

		public override ISprite MakeInstance(Gob owner, byte[] state)
		{
			var reader = new BinaryDataReader(state);

			var value = reader.ReadInt32();
			var displayPlus = reader.ReadByte() != 0;

			var str = value.ToString();
			if (displayPlus && value > 0)
			{
				str = "+" + str;
			}
			var color = UnpackColor(reader.ReadUInt16());

			return new FloatText(str, color);
		}

		private static Color UnpackColor(ushort value)
		{
			var r = (value & 0xF000) >> 12;
			var g = (value & 0xF00) >> 8;
			var b = (value & 0xF0) >> 4;
			var a = (value & 0xF);
			return Color.FromArgb(
				(byte)((a << 4) | a),
				(byte)((r << 4) | r),
				(byte)((g << 4) | g),
				(byte)((b << 4) | b));
		}
	}
}
