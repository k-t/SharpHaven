using System;
using System.Collections.Generic;
using MonoHaven.Game;

namespace MonoHaven.UI.Remote
{
	public class WidgetAdapterRegistry
	{
		private readonly Dictionary<string, WidgetAdapter> adapters;

		public WidgetAdapterRegistry(GameSession session)
		{
			adapters = new Dictionary<string, WidgetAdapter>();
			adapters["cnt"] = new ContainerAdapter();
			adapters["img"] = new ImageAdapter();
			adapters["charlist"] = new CharlistAdapter(session);
			adapters["ibtn"] = new ImageButtonAdapter();
			adapters["wnd"] = new WindowAdapter();
			adapters["av2"] = new AvatarViewAdapter(session);
			adapters["lbl"] = new LabelAdapter();
			adapters["btn"] = new ButtonAdapter();
		}

		public WidgetAdapter Get(string widgetType)
		{
			WidgetAdapter adapter;
			if (adapters.TryGetValue(widgetType, out adapter))
				return adapter;
			throw new ArgumentException("Unknown widget type " + widgetType);
		}
	}
}
