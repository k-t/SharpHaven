using OpenTK.Input;

namespace MonoHaven.Input
{
	public class KeyEvent : InputEvent
	{
		private readonly Key key;

		public KeyEvent(Key key)
		{
			this.key = key;
		}

		public Key Key
		{
			get { return key; }
		}
	}
}
