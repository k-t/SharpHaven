using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerMeter : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var metrics = new List<Metric>();

			var resName = (string)args[0];
			for (int i = 1; i < args.Length; i += 2)
				metrics.Add(new Metric((Color)args[i], (int)args[i + 1]));
			
			var widget = new Meter(parent.Widget);
			widget.Background = App.Resources.Get<Drawable>(resName);
			widget.SetMetrics(metrics);
			return new ServerMeter(id, parent, widget);
		}

		private readonly Meter widget;

		public ServerMeter(ushort id, ServerWidget parent, Meter widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "set")
			{
				var metrics = new List<Metric>();
				for (int i = 0; i < args.Length; i += 2)
					metrics.Add(new Metric((Color)args[i], (int)args[i + 1]));
				widget.SetMetrics(metrics);
			}
			else if (message == "tt")
			{
				var text = (string)args[0];
				widget.Tooltip = new Tooltip(text);
			}
			else
				base.ReceiveMessage(message, args);
		}
	}
}
