using System.Drawing;

namespace MonoHaven.UI.Remote
{
	public class ContainerAdapter : WidgetAdapter
	{
		public override Widget Create(Widget parent, object[] args)
		{
			var widget = new Container(parent);
			if (args.Length > 0)
			{
				var size = (Point)args[0];
				widget.SetSize(size.X, size.Y);
			}
			return widget;
		}
	}
}
