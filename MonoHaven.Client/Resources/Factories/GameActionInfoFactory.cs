using MonoHaven.Game;
using MonoHaven.Graphics;

namespace MonoHaven.Resources
{
	public class GameActionInfoFactory : IObjectFactory<GameActionInfo>
	{
		public GameActionInfo Create(string resName, Resource res)
		{
			var data = res.GetLayer<ActionData>();
			var image = TextureSlice.FromBitmap(res.GetLayer<ImageData>().Data);
			return new GameActionInfo(
				data.Name,
				data.Parent,
				data.Name,
				new Picture(image, null),
				data.Verbs);
		}
	}
}
