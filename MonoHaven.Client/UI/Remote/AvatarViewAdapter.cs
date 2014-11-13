using System.Linq;
using MonoHaven.Game;

namespace MonoHaven.UI.Remote
{
	public class AvatarViewAdapter : WidgetAdapter
	{
		private readonly GameSession session;

		public AvatarViewAdapter(GameSession session)
		{
			this.session = session;
		}

		public override Widget Create(Widget parent, object[] args)
		{
			var layers = args.Select(x => session.GetResource((int)x));
			return new AvatarView(parent, layers);
		}
	}
}
