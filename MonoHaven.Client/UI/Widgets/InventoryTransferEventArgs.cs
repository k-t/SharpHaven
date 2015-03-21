using System;
using MonoHaven.Input;

namespace MonoHaven.UI.Widgets
{
	public class InventoryTransferEventArgs : EventArgs
	{
		private readonly int delta;
		private readonly KeyModifiers mods;

		public InventoryTransferEventArgs(int delta, KeyModifiers mods)
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
