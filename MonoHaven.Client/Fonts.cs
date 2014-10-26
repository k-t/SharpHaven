using MonoHaven.Graphics;
using MonoHaven.Resources;
using SharpFont;

namespace MonoHaven
{
	public static class Fonts
	{
		private static readonly Library fontLibrary;

		static Fonts()
		{
			fontLibrary = new Library();
			
			var fontBytes = EmbeddedResource.GetBytes("Fonts.NotoSans-Regular.ttf");
			var fontFace = fontLibrary.NewMemoryFace(fontBytes, 0);
			var font = new SpriteFont(fontFace, 14);

			Default = font;
		}

		public static SpriteFont Default { get; private set; }
	}
}
