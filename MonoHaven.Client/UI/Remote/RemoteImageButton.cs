﻿namespace MonoHaven.UI.Remote
{
	public class RemoteImageButton : RemoteWidget
	{
		private readonly ImageButton widget;

		public RemoteImageButton(int id, RemoteWidget parent, object[] args)
			: base(id)
		{
			widget = new ImageButton(parent.Widget);

			if (args.Length > 0)
				widget.Up = App.Instance.Resources.GetTexture((string)args[0]);

			if (args.Length > 1)
				widget.Down = App.Instance.Resources.GetTexture((string)args[1]);
			else
				widget.Down = App.Instance.Resources.GetTexture((string)args[0]);
		}

		public override Widget Widget
		{
			get { return widget; }
		}
	}
}
