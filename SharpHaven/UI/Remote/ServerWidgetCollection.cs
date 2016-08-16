using System.Collections.Generic;
using Haven;
using NLog;

namespace SharpHaven.UI.Remote
{
	public class ServerWidgetCollection
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly Dictionary<ushort, ServerWidget> widgets;

		public ServerWidgetCollection()
		{
			widgets = new Dictionary<ushort, ServerWidget>();
		}

		public ServerWidget this[ushort id]
		{
			get
			{
				ServerWidget widget;
				return widgets.TryGetValue(id, out widget) ? widget : null;
			}
		}

		public void Add(ServerWidget widget)
		{
			widgets[widget.Id] = widget;
		}

		public void Remove(ushort id)
		{
			ServerWidget widget;
			if (!widgets.Remove(id, out widget))
			{
				Log.Warn("Attempt to remove non-existent widget {0}", id);
				return;
			}

			widget.Remove();
			widget.Dispose();

			foreach (var child in widget.Descendants)
			{
				widgets.Remove(child.Id);
				child.Dispose();
			}
		}
	}
}
