using System.Drawing;
using SharpHaven.Net;
using SharpHaven.Resources;

namespace SharpHaven.Graphics.Sprites.Fx
{
	public class FloatTextMaker : SpriteMaker
	{
		public FloatTextMaker(Resource res)
			: base(res)
		{
		}

		public override ISprite MakeInstance(byte[] state)
		{
			var reader = new MessageReader(0, state);

			var value = reader.ReadInt32();
			var displayPlus = reader.ReadByte() != 0;

			var str = value.ToString();
			if (displayPlus && value > 0)
			{
				str = "+" + str;
			}
			var color = UnpackColor(reader.ReadUint16());

			return new FloatText(str, color);
		}

		private static Color UnpackColor(ushort value)
		{
			int r = (value & 0xF000) >> 12;
			int g = (value & 0xF00) >> 8;
			int b = (value & 0xF0) >> 4;
			int a = (value & 0xF);
			return Color.FromArgb((a << 4) | a, (r << 4) | r, (g << 4) | g, (b << 4) | b);
		}
	}
}
