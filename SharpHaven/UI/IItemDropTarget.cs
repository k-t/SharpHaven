using OpenTK.Input;
using SharpHaven.Graphics;

namespace SharpHaven.UI
{
	public interface IItemDropTarget
	{
		bool Drop(Coord2d p, Coord2d ul, KeyModifiers mods);
		bool Interact(Coord2d p, Coord2d ul, KeyModifiers mods);
	}
}
