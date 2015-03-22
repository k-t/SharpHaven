using MonoHaven.Resources;

namespace MonoHaven.Messages
{
	public class ActionMessage
	{
		public bool RemoveFlag { get; set; }
		public ResourceRef Resource { get; set; }
	}
}
