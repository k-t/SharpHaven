using System;

namespace MonoHaven.UI.Remote
{
	public class CharlistController : Controller
	{
		private readonly Widget widget;

		private CharlistController(int id, Widget widget) : base(id)
		{
			this.widget = widget;
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static Controller Create(int id, Controller parent, object[] args)
		{
			return new CharlistController(id, new Container(parent.Widget));
		}
	}
}
