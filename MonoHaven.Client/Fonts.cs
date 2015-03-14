using MonoHaven.Graphics;

namespace MonoHaven
{
	public static class Fonts
	{
		static Fonts()
		{
			var sans = App.Resources.GetFont("custom/fonts/sans");
			var serif = App.Resources.GetFont("custom/fonts/serif");
			Default = new SpriteFont(sans, 14);
			LabelText = new SpriteFont(sans, 11);
			Text = new SpriteFont(serif, 12);
			Heading = new SpriteFont(serif, 18);
		}

		public static SpriteFont Default
		{
			get;
			private set;
		}

		public static SpriteFont LabelText
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
	}
}
