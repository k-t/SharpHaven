using SharpHaven.Resources;

namespace SharpHaven.Game.Events
{
	public class GameActionsUpdateEvent
	{
		public ResourceRef[] Added
		{
			get;
			set;
		}

		public ResourceRef[] Removed
		{
			get;
			set;
		}
	}
}
