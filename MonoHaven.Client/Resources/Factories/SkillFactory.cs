using System.Linq;
using MonoHaven.Game;
using MonoHaven.Graphics;

namespace MonoHaven.Resources
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
			var tooltipData = res.GetLayer<TooltipData>();
			if (tooltipData != null)
				tooltip = tooltipData.Text;

			string id = resName.Split('/').Last();
			
			var skill = new Skill(id, image, tooltip);
			return skill;
		}
	}
}
