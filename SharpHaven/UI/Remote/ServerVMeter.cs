using SharpHaven.Graphics;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerVMeter : ServerWidget
	{
		private VMeter widget;

		public ServerVMeter(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("set", Set);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerVMeter(id, parent);
		}

		protected override void OnInit(Coord2d position, object[] args)
		{
			var amount = (int)args[0];
			var color = args.Length > 4
				? Color.FromArgb((byte)args[1], (byte)args[2], (byte)args[3], (byte)args[4])
				: Color.FromArgb((byte)args[1], (byte)args[2], (byte)args[3]);

			widget = new VMeter(Parent.Widget);
			widget.Move(position);
			widget.Amount = amount;
			widget.Color = color;
		}

		private void Set(object[] args)
		{
			widget.Amount = (int)args[0];
		}
	}
}
