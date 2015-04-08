using System;

namespace MonoHaven.UI.Widgets
{
	public class BeliefChangeEvent : EventArgs
	{
		public BeliefChangeEvent(string name, int delta, bool invert)
		{
			Name = name;
			Delta = delta * (invert ? -1 : 1);
		}

		public string Name
		{
			get;
			protected set;
		}

		public int Delta
		{
			get;
			protected set;
		}
	}
}
