using MonoHaven.Graphics;
using MonoHaven.Resources;

namespace MonoHaven.Game
{
	public class Skill
	{
		private readonly SkillInfo info;
		private readonly int? cost;

		public Skill(SkillInfo info, int? cost = null)
		{
			this.info = info;
			this.cost = cost;
		}

		public string Id
		{
			get { return info.Id; }
		}

		public Drawable Image
		{
			get { return info.Image; }
		}

		public string Tooltip
		{
			get { return info.Tooltip; }
		}

		public int? Cost
		{
			get { return cost; }
		}
	}
}
