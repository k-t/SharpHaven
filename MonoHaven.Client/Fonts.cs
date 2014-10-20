﻿using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using MonoHaven.Resources;

namespace MonoHaven
{
	public static class Fonts
	{
		static Fonts()
		{
			var fc = new PrivateFontCollection();

			fc.LoadFromResource("Fonts.PTC55F.ttf");
			fc.LoadFromResource("Fonts.PTS55F.ttf");

			Text = new Font(fc.FindFont("PT Sans"), 12);
			Caption = new Font(fc.FindFont("PT Sans Caption"), 12);
		}

		public static Font Caption { get; private set; }
		public static Font Text { get; private set; }

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
