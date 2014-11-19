using System.Collections.Generic;
using MonoHaven.Graphics.Sprites;

namespace MonoHaven.UI.Remote
{
	public class ServerCharlist : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var height = (int)args[0];
			var widget = new Charlist(parent.Widget, height);
			return new ServerCharlist(id, parent, widget);
		}

		private readonly Charlist widget;

		public ServerCharlist(ushort id, ServerWidget parent, Charlist widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.CharacterSelected += OnCharacterSelected;
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "add")
			{
				var name = (string)args[0];
				var layers = new List<Delayed<Sprite>>();
				for (int i = 1; i < args.Length; i++)
					layers.Add(Session.GetSprite((int)args[i]));
				widget.AddChar(name, layers);
			}
			else
				base.ReceiveMessage(message, args);
		}

		private void OnCharacterSelected(string charName)
		{
			SendMessage("play", charName);
		}
	}
}
