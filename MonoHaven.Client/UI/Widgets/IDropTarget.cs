using System.Drawing;
using MonoHaven.Input;

namespace MonoHaven.UI.Widgets
{
	public interface IDropTarget
	{
		bool Drop(Point p, Point ul, KeyModifiers mods);
		bool ItemInteract(Point p, Point ul, KeyModifiers mods);
	}
}
