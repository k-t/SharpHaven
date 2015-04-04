using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerSpeedget : ServerWidget
	{
		private Speedget widget;

		public ServerSpeedget(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("cur", SetCurrent);
			SetHandler("max", SetMax);
		}
		
		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerSpeedget(id, parent);
		}

		protected override void OnInit(object[] args)
		{
			var cur = (int)args[0];
			var max = (int)args[1];

			widget = new Speedget(Parent.Widget);
			widget.CurrentSpeed = cur;
			widget.MaxSpeed = max;
			widget.SpeedSelected += value => SendMessage("set", value);
		}

		private void SetCurrent(object[] args)
		{
			widget.CurrentSpeed = (int)args[0];
		}

		private void SetMax(object[] args)
		{
			widget.MaxSpeed = (int)args[0];
		}
	}
}
