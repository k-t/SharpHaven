#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

namespace MonoHaven.UI
{
	public interface IWidgetHost
	{
		void RequestKeyboardFocus(Widget widget);
		void GrabMouse(Widget widget);
		void ReleaseMouse();
	}
}
