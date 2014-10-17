using System;
using System.Drawing;
using System.Reflection;

namespace MonoHaven
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			using (var iconImage = LoadIcon())
			using (var icon = Icon.FromHandle(iconImage.GetHicon()))
			using (var gameWindow = new MainWindow(800, 600))
			{
				gameWindow.Icon = icon;
				gameWindow.Run();
			}
		}

		private static Bitmap LoadIcon()
		{
			var stream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream("MonoHaven.Resources.icon.png");
			using (stream)
			{
				return (Bitmap)Image.FromStream(stream);
			}
		}
	}
}
