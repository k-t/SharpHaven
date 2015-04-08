using C5;
using MonoHaven.Game;
using NLog;

namespace MonoHaven.UI.Remote
{
	public class ServerWidgetCollection
	{
		private static readonly NLog.Logger Log = LogManager.GetCurrentClassLogger();

		private readonly TreeDictionary<ushort, ServerWidget> widgets;

		public ServerWidgetCollection(GameSession session)
		{
			widgets = new TreeDictionary<ushort, ServerWidget>();
		}

		public void Add(ServerWidget widget)
		{
			widgets[widget.Id] = widget;
		}

		public ServerWidget Get(ushort id)
		{
			ServerWidget widget;
			return widgets.Find(ref id, out widget) ? widget : null;
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
