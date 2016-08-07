using System.Collections.Generic;
using SharpHaven.Client;
using SharpHaven.Graphics;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerMeter : ServerWidget
	{
		private Meter widget;

		public ServerMeter(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("set", Set);
			SetHandler("tt", SetTooltip);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerMeter(id, parent);
		}

		protected override void OnInit(Coord2d position, object[] args)
		{
			var metrics = new List<Metric>();

			var resName = (string)args[0];
			for (int i = 1; i < args.Length; i += 2)
				metrics.Add(new Metric((Color)args[i], (int)args[i + 1]));

			widget = new Meter(Parent.Widget);
			widget.Move(position);
			widget.Background = App.Resources.Get<Drawable>(resName);
			widget.SetMetrics(metrics);
		}

		private void Set(object[] args)
		{
			var metrics = new List<Metric>();
			for (int i = 0; i < args.Length; i += 2)
				metrics.Add(new Metric((Color)args[i], (int)args[i + 1]));
			widget.SetMetrics(metrics);
		}

		private void SetTooltip(object[] args)
		{
			var text = (string)args[0];
			widget.Tooltip = new Tooltip(text);
		}
	}
}
