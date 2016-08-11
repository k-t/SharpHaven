using Haven;
using OpenTK.Input;

namespace SharpHaven.UI
{
	public interface IItemDropTarget
	{
		bool Drop(Point2D p, Point2D ul, KeyModifiers mods);
		bool Interact(Point2D p, Point2D ul, KeyModifiers mods);
	}
}
