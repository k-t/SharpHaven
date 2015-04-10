﻿using System.Drawing;
using System.Linq;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerLayeredAvatarView : ServerWidget
	{
		private AvatarView widget;

		public ServerLayeredAvatarView(ushort id, ServerWidget parent)
			: base(id, parent)
		{
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerLayeredAvatarView(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			var session = Parent.Session;
			var layers = args.Select(x => session.GetSprite((int)x));
			
			widget = new AvatarView(Parent.Widget);
			widget.Move(position);
			widget.Avatar = new Avatar(layers);
		}
	}
}
