﻿namespace MonoHaven.Resources
{
	public class ActionData
	{
		public string Name { get; set; }
		public string Prerequisite { get; set; }
		public ResourceRef Parent { get; set; }
		public char Hotkey { get; set; }
		public string[] Verbs { get; set; }
	}
}
