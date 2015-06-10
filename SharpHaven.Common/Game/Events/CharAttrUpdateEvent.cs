namespace SharpHaven.Game.Events
{
	public class CharAttrUpdateEvent
	{
		public string Name { get; set; }
		public int BaseValue { get; set; }
		public int CompValue { get; set; }
	}
}
