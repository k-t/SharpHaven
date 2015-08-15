namespace SharpHaven.Input
{
	public class KeyPressEvent : InputEvent
	{
		public KeyPressEvent(char keyChar)
		{
			KeyChar = keyChar;
		}

		public char KeyChar { get; }
	}
}
