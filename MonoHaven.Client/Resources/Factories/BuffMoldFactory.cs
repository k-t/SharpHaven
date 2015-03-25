using MonoHaven.Game;
using MonoHaven.Graphics;

namespace MonoHaven.Resources
{
	public class BuffMoldFactory : IObjectFactory<BuffMold>
	{
		private readonly IObjectFactory<Drawable> imageFactory;

		public BuffMoldFactory(IObjectFactory<Drawable> imageFactory)
		{
			this.imageFactory = imageFactory;
		}

		public BuffMold Create(string resName, Resource res)
		{
			var image = imageFactory.Create(resName, res);
			var tooltip = res.GetLayer<TooltipData>();
			return new BuffMold(image, tooltip != null ? tooltip.Text : null);
		}
	}
}
