using System.Drawing;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public interface IItemDropTarget
	{
		bool Drop(Point p, Point ul, KeyModifiers mods);
		bool Interact(Point p, Point ul, KeyModifiers mods);
	}
}
