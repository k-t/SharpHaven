using OpenTK.Input;

namespace MonoHaven.Input
{
	public abstract class InputEvent
	{
		private readonly KeyModifiers mods;

		protected InputEvent()
		{
			mods = GetCurrentKeyModifiers();
		}

		public bool Handled
		{
			get;
			set;
		}

		public KeyModifiers Modifiers
		{
			get { return mods; }
		}

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
