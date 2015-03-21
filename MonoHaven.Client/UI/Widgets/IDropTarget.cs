using System.Drawing;

namespace MonoHaven.UI.Widgets
{
	public interface IDropTarget
	{
		bool Drop(Point p, Point ul);
		bool ItemInteract(Point p, Point ul);
	}
}
