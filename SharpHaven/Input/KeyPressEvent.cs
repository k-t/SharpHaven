namespace SharpHaven.Input
{
	public class KeyPressEvent : InputEvent
	{
		private readonly char keyChar;

		public KeyPressEvent(char keyChar)
		{
			this.keyChar = keyChar;
		}

		public char KeyChar
		{
			get { return keyChar; }
		}
	}
}
