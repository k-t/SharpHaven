using SharpHaven.Client;
using SharpHaven.Graphics;

namespace SharpHaven.Resources
{
	public class ItemMoldFactory : IObjectFactory<ItemMold>
	{
		private readonly IObjectFactory<Drawable> imageFactory;

		public ItemMoldFactory(IObjectFactory<Drawable> imageFactory)
		{
			this.imageFactory = imageFactory;
		}

		public ItemMold Create(string resName, Resource res)
		{
			var image = imageFactory.Create(resName, res);
			var tooltip = res.GetLayer<TooltipData>();
			return new ItemMold(image, tooltip != null ? tooltip.Text : null);
		}
	}
}
