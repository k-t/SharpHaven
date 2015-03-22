using System.Linq;
using MonoHaven.Game;
using MonoHaven.Graphics;

namespace MonoHaven.Resources
{
	public class SkillInfoFactory : IObjectFactory<SkillInfo>
	{
		private readonly IObjectFactory<Drawable> imageFactory;

		public SkillInfoFactory(IObjectFactory<Drawable> imageFactory)
		{
			this.imageFactory = imageFactory;
		}

		public SkillInfo Create(string resName, Resource res)
		{
			var image = imageFactory.Create(resName, res);

			string tooltip = null;
			var tooltipData = res.GetLayer<TooltipData>();
			if (tooltipData != null)
				tooltip = tooltipData.Text;

			string id = resName.Split('/').Last();
			
			var skill = new SkillInfo(id, image, tooltip);
			return skill;
		}
	}
}
