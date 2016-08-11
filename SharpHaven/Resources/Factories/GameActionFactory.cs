using Haven.Resources;
using SharpHaven.Client;
using SharpHaven.Graphics;

namespace SharpHaven.Resources
{
	public class GameActionFactory : IObjectFactory<GameAction>
	{
		public GameAction Create(string resName, Resource res)
		{
			var data = res.GetLayer<ActionLayer>();
			var image = TextureSlice.FromBitmap(res.GetLayer<ImageLayer>().Data);
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
