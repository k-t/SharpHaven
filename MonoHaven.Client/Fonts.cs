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

			var sans = LoadFace("Fonts.SourceSansPro-Regular.ttf");
			var serif = LoadFace("Fonts.SourceSerifPro-Regular.ttf");

			Default = new SpriteFont(sans, 14);
			Text = new SpriteFont(serif, 12);
			Heading = new SpriteFont(serif, 18);
		}

		public static SpriteFont Default
		{
			get;
			private set;
		}

		public static SpriteFont Text
		{
			get;
			private set;
		}

		public static SpriteFont Heading
		{
			get;
			private set;
		}

		private static Face LoadFace(string resName)
		{
			var bytes = EmbeddedResource.GetBytes(resName);
			return fontLibrary.NewMemoryFace(bytes, 0);
		}
	}
}
