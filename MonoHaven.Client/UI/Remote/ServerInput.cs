#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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
