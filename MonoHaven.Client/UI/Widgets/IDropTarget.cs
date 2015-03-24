using System.Drawing;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public interface IDropTarget
	{
		bool Drop(Point p, Point ul, KeyModifiers mods);
		bool ItemInteract(Point p, Point ul, KeyModifiers mods);
	}
}
