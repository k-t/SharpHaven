using System;
using System.Drawing;
using System.Reflection;
using System.IO;
using MonoHaven.Resources;

namespace MonoHaven
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var resSrc = new ZipSource("haven-res.jar");
			ResLoader.Current = new SimpleResourceLoader(resSrc);

			using (var iconImage = LoadIcon())
			using (var icon = Icon.FromHandle(iconImage.GetHicon()))
			using (var gameWindow = new HavenWindow(800, 600))
			{
				gameWindow.Icon = icon;
				gameWindow.Run(30, 30);
			}
		}

		private static Bitmap LoadIcon()
		{
			var stream = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceStream("MonoHaven.Resources.icon.png");
			using (stream)
			{
				return (Bitmap)Bitmap.FromStream(stream);
			}
		}
	}
}
