using OpenTK.Input;
using SharpHaven.Graphics;

namespace SharpHaven.UI
{
	public interface IItemDropTarget
	{
		bool Drop(Coord2D p, Coord2D ul, KeyModifiers mods);
		bool Interact(Coord2D p, Coord2D ul, KeyModifiers mods);
	}
}
