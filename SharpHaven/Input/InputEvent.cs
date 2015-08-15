using System;
using OpenTK.Input;

namespace SharpHaven.Input
{
	public abstract class InputEvent : EventArgs
	{
		protected InputEvent()
		{
			Modifiers = GetCurrentKeyModifiers();
		}

		public bool Handled { get; set; }

		public KeyModifiers Modifiers { get; }

		private static KeyModifiers GetCurrentKeyModifiers()
		{
			var mods = (KeyModifiers)0;
			var keyboardState = Keyboard.GetState();

			if (keyboardState.IsKeyDown(Key.LShift) ||
				keyboardState.IsKeyDown(Key.RShift))
				mods |= KeyModifiers.Shift;

			if (keyboardState.IsKeyDown(Key.LAlt) ||
				keyboardState.IsKeyDown(Key.RAlt))
				mods |= KeyModifiers.Alt;

			if (keyboardState.IsKeyDown(Key.ControlLeft) ||
				keyboardState.IsKeyDown(Key.ControlRight))
				mods |= KeyModifiers.Control;

			return mods;
		}
	}
}
