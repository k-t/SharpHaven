using System;
using OpenTK.Input;

namespace SharpHaven.UI.Widgets
{
	public class TransferEvent : EventArgs
	{
		public TransferEvent(int delta, KeyModifiers mods)
		{
			Delta = delta;
			KeyModifiers = mods;
		}

		public int Delta { get; }

		public KeyModifiers KeyModifiers { get; }
	}
}
