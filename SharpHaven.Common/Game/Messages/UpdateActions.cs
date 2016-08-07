using SharpHaven.Resources;

namespace SharpHaven.Game.Messages
{
	public class UpdateActions
	{
		public ResourceRef[] Added { get; set; }

		public ResourceRef[] Removed { get; set; }
	}
}