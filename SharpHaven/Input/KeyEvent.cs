using OpenTK.Input;

namespace SharpHaven.Input
{
	public class KeyEvent : InputEvent
	{
		public KeyEvent(Key key)
		{
			Key = key;
		}

		public Key Key { get; }
	}
}
