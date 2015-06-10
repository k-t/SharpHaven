using SharpHaven.Resources;

namespace SharpHaven.Game.Events
{
	public class ActionUpdateEvent
	{
		public bool RemoveFlag { get; set; }
		public ResourceRef Resource { get; set; }
	}
}
