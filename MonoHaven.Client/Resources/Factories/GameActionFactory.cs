using MonoHaven.Game;
using MonoHaven.Graphics;

namespace MonoHaven.Resources
{
	public class GameActionFactory : IObjectFactory<GameAction>
	{
		public GameAction Create(string resName, Resource res)
		{
			var data = res.GetLayer<ActionData>();
			var image = TextureSlice.FromBitmap(res.GetLayer<ImageData>().Data);
			return new GameAction(
				data.Name,
				resName,
				data.Parent,
				data.Name,
				new Picture(image, null),
				data.Verbs);
		}
	}
}
