using OpenTK.Input;

namespace MonoHaven.UI
{
	public class KeyEventArgs : KeyboardKeyEventArgs
	{
		public KeyEventArgs(KeyboardKeyEventArgs keyEventArgs)
			: base(keyEventArgs)
		{}

		public bool Handled { get; set; }
	}
}
