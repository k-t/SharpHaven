using System;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class BeltClickEvent : EventArgs
	{
		private readonly int slot;
		private readonly MouseButton button;
		private readonly KeyModifiers mods;

		public BeltClickEvent(int slot, MouseButton button, KeyModifiers mods)
		{
			this.slot = slot;
			this.button = button;
			this.mods = mods;
		}

		public int Slot
		{
			get { return slot; }
		}

		public MouseButton Button
		{
			get { return button; }
		}

		public KeyModifiers Modifiers
		{
			get { return mods; }
		}
	}
}
