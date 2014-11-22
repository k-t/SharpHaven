#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;
using MonoHaven.Utils;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public class RootWidget : Widget
	{
		public RootWidget(IWidgetHost host) : base(host)
		{
		}

		public event Action<KeyboardKeyEventArgs> UnhandledKeyDown;
		public event Action<KeyboardKeyEventArgs> UnhandledKeyUp;

		protected override bool OnKeyDown(KeyboardKeyEventArgs e)
		{
			UnhandledKeyDown.Raise(e);
			return true;
		}

		protected override bool OnKeyUp(KeyboardKeyEventArgs e)
		{
			UnhandledKeyUp.Raise(e);
			return true;
		}
	}
}
