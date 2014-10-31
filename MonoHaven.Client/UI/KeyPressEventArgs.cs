namespace MonoHaven.UI
{
	public class KeyPressEventArgs : OpenTK.KeyPressEventArgs
	{
		public KeyPressEventArgs(char c)
			: base(c)
		{ }

		public bool Handled { get; set; }
	}
}
