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
			
			var sans = LoadFace("Fonts.NotoSans-Regular.ttf");
			var serif = LoadFace("Fonts.NotoSerif-Regular.ttf");

			Default = new SpriteFont(sans, 14);
			ButtonText = new SpriteFont(serif, 12);
			Heading = new SpriteFont(serif, 18);
		}

		public static SpriteFont Default
		{
			get;
			private set;
		}

		public static SpriteFont ButtonText
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
