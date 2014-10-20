using System.Drawing;
using MonoHaven.Resources;

namespace MonoHaven
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			using (var iconImage = EmbeddedResource.GetImage("icon.png"))
			using (var icon = Icon.FromHandle(iconImage.GetHicon()))
			using (var gameWindow = new MainWindow(800, 600))
			{
				gameWindow.Icon = icon;
				gameWindow.Run();
			}
		}
	}
}
