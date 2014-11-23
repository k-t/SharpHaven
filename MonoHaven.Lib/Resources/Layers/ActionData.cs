namespace MonoHaven.Resources.Layers
{
	public class ActionData : IDataLayer
	{
		public string Name { get; set; }
		public ResourceRef Parent { get; set; }
		public char Hotkey { get; set; }
		public string[] Verbs { get; set; }
	}
}
