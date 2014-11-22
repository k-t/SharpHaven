using OpenTK.Input;

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
	}
}
