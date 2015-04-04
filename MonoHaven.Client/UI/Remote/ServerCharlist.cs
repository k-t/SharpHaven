using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Graphics.Sprites;
using MonoHaven.UI.Widgets;
using MonoHaven.Utils;

namespace MonoHaven.UI.Remote
{
	public class ServerCharlist : ServerWidget
	{
		private Charlist widget;

		public ServerCharlist(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("add", AddCharacter);
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerCharlist(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			var height = (int)args[0];

			widget = new Charlist(Parent.Widget, height);
			widget.Move(position);
			widget.CharacterSelected += OnCharacterSelected;
		}

		private void AddCharacter(object[] args)
		{
			var name = (string)args[0];
			var layers = new List<Delayed<ISprite>>();
			for (int i = 1; i < args.Length; i++)
				layers.Add(Session.GetSprite((int)args[i]));
			widget.AddChar(name, layers);
		}

		private void OnCharacterSelected(string charName)
		{
			SendMessage("play", charName);
		}
	}
}
