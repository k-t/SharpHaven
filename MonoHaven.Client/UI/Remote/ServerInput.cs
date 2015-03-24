using OpenTK.Input;
using MonoHaven.Utils;

namespace MonoHaven.UI.Remote
{
	public static class ServerInput
	{
		public static int ToServerButton(MouseButton button)
		{
			switch (button)
			{
				case MouseButton.Left:
					return 1;
				case MouseButton.Middle:
					return 2;
				case MouseButton.Right:
					return 3;
				default:
					return 0;
			}
		}

		public static int ToServerModifiers(KeyModifiers mods)
		{
			int result = 0;
			if (mods.HasShift()) result |= 1;
			if (mods.HasControl()) result |= 2;
			if (mods.HasAlt()) result |= 4;
			return result;
		}
	}
}
