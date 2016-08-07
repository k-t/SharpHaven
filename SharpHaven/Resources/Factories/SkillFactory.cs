using System.Linq;
using SharpHaven.Client;
using SharpHaven.Graphics;

namespace SharpHaven.Resources
{
	public class SkillFactory : IObjectFactory<Skill>
	{
		private readonly IObjectFactory<Drawable> imageFactory;

		public SkillFactory(IObjectFactory<Drawable> imageFactory)
		{
			this.imageFactory = imageFactory;
		}

		public Skill Create(string resName, Resource res)
		{
			var image = imageFactory.Create(resName, res);

			string tooltip = null;
			var tooltipData = res.GetLayer<TooltipLayer>();
			if (tooltipData != null)
				tooltip = tooltipData.Text;

			string id = resName.Split('/').Last();
			
			var skill = new Skill(id, image, tooltip);
			return skill;
		}
	}
}
