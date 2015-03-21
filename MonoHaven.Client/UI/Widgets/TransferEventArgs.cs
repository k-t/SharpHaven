using System;
using MonoHaven.Input;

namespace MonoHaven.UI.Widgets
{
	public class TransferEventArgs : EventArgs
	{
		private readonly int delta;
		private readonly KeyModifiers mods;

		public TransferEventArgs(int delta, KeyModifiers mods)
		{
			this.delta = delta;
			this.mods = mods;
		}

		public int Delta
		{
			get { return delta; }
		}

		public KeyModifiers Modifiers
		{
			get { return mods; }
		}
	}
}
