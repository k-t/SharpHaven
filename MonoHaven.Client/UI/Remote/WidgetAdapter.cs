using System;
using NLog;

namespace MonoHaven.UI.Remote
{
	public abstract class WidgetAdapter
	{
		private readonly static Logger log = LogManager.GetCurrentClassLogger();

		public abstract Widget Create(Widget parent, object[] args);

		public virtual void HandleMessage(Widget widget, string message, object[] args)
		{
			// TODO: handle common widget commands (focus, tab, etc).

			log.Warn("Unhandled message {0}({1}) for {2}",
				message, string.Join(",", args), widget.GetType());
		}

		public virtual void SetEventHandler(Widget widget, Action<string, object[]> handler)
		{
		}
	}
}
