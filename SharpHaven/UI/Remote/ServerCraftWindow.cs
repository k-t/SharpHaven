using System.Drawing;
using SharpHaven.Client;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerCraftWindow : ServerWindow
	{
		private CraftWindow widget;

		public ServerCraftWindow(ushort id, ServerWidget parent)
			: base(id, parent)
		{
			SetHandler("pop", Pop);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static new ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerCraftWindow(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			var recipeName = (string)args[0];

			widget = new CraftWindow(Parent.Widget);
			widget.Move(position);
			widget.RecipeName = recipeName;
			widget.Craft += () => SendMessage("make", 0);
			widget.CraftAll += () => SendMessage("make", 1);
		}

		private void Pop(object[] args)
		{
			widget.Clear();
			int i;
			for (i = 0; (int)args[i] >= 0; i += 2)
			{
				var resId = (int)args[i];
				var count = (int)args[i + 1];
				widget.AddInput(MakeItem(resId, count));
			}
			for (i++; (i < args.Length) && ((int)args[i] >= 0); i += 2)
			{
				var resId = (int)args[i];
				var count = (int)args[i + 1];
				widget.AddOutput(MakeItem(resId, count));
			}
		}

		private Item MakeItem(int resId, int count)
		{
			var mold = Session.Resources.Get<ItemMold>(resId);
			return new Item(mold) { Amount = count };
		}
	}
}
