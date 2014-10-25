using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using QuickFont;
using MonoHaven.Resources;

namespace MonoHaven
{
	public static class Fonts
	{
		static Fonts()
		{
			var fc = new PrivateFontCollection();

			fc.LoadFromResource("Fonts.NotoSans-Regular.ttf");

			var font = new QFont(new Font(fc.FindFont("Noto Sans"), 10));

			Text = font;
			Caption = font;
		}

		public static QFont Caption { get; private set; }
		public static QFont Text { get; private set; }

		private static FontFamily FindFont(this PrivateFontCollection fc, string fontName)
		{
			return fc.Families.FirstOrDefault(x => x.Name == fontName);
		}

		private static void LoadFromResource(this PrivateFontCollection fc, string name)
		{
			byte[] data = EmbeddedResource.GetBytes(name);
			unsafe
			{
				fixed (byte* pFontData = data)
					fc.AddMemoryFont((System.IntPtr)pFontData, data.Length);
			}
		}
	}
}
