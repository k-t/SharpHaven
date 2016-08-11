using Haven.Resources;
using SharpHaven.Client;
using SharpHaven.Graphics;

namespace SharpHaven.Resources
{
	public class BuffProtoFactory : IObjectFactory<BuffProto>
	{
		private readonly IObjectFactory<Drawable> imageFactory;

		public BuffProtoFactory(IObjectFactory<Drawable> imageFactory)
		{
			this.imageFactory = imageFactory;
		}

		public BuffProto Create(string resName, Resource res)
		{
			var image = imageFactory.Create(resName, res);
			var tooltip = res.GetLayer<TooltipLayer>();
			return new BuffProto(image, tooltip?.Text);
		}
	}
}
