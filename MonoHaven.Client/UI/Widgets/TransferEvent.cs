using System;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class TransferEvent : EventArgs
	{
		private readonly int delta;
		private readonly KeyModifiers mods;

		public TransferEvent(int delta, KeyModifiers mods)
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
