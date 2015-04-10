using SharpHaven.Resources;

namespace SharpHaven.Messages
{
	public class ActionMessage
	{
		public bool RemoveFlag { get; set; }
		public ResourceRef Resource { get; set; }
	}
}
