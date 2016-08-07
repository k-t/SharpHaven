using SharpHaven.Client;
using SharpHaven.Graphics;

namespace SharpHaven.Resources
{
	public class ItemProtoFactory : IObjectFactory<ItemProto>
	{
		private readonly IObjectFactory<Drawable> imageFactory;

		public ItemProtoFactory(IObjectFactory<Drawable> imageFactory)
		{
			this.imageFactory = imageFactory;
		}

		public ItemProto Create(string resName, Resource res)
		{
			var image = imageFactory.Create(resName, res);
			var tooltip = res.GetLayer<TooltipLayer>();
			return new ItemProto(image, tooltip?.Text);
		}
	}
}
