using System;
using OpenTK.Input;

namespace SharpHaven.UI.Widgets
{
	public class BeltClickEvent : EventArgs
	{
		public BeltClickEvent(int slot, MouseButton button, KeyModifiers mods)
		{
			Slot = slot;
			Button = button;
			Modifiers = mods;
		}

		public int Slot { get; }

		public MouseButton Button { get; }

		public KeyModifiers Modifiers { get; }
	}
}
