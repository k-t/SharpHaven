namespace SharpHaven.Game.Events
{
	public class WidgetMessageEvent
	{
		public ushort WidgetId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public object[] Args
		{
			get;
			set;
		}
	}
}
