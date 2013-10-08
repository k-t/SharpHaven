using System;
using System.Drawing;
using System.Collections.Generic;
using MonoHaven.Resources;
using Widgets = MonoHaven.Widgets;
using OpenTK.Input;

namespace MonoHaven
{
	public class UI : IDisposable
	{
		private bool disposed;
		private readonly List<Widget> widgets;

		private Tex back;
		private Tex logo;
		private Tex ubtn;
		private Tex dbtn;

		public UI()
		{
			this.widgets = new List<Widget>();

			this.back = Tex.FromImage(ResLoader.Current.LoadImage("gfx/loginscr"));
			this.logo = Tex.FromImage(ResLoader.Current.LoadImage("gfx/logo"));

			this.ubtn = Tex.FromImage(ResLoader.Current.LoadImage("gfx/hud/buttons/loginu"));
			this.dbtn = Tex.FromImage(ResLoader.Current.LoadImage("gfx/hud/buttons/logind"));

			this.widgets.Add(new Widgets.Image(Point.Empty, this.back));
			this.widgets.Add(new Widgets.Image(new Point(420 - logo.Width / 2, 215 - logo.Height / 2), this.logo));
			this.widgets.Add(new Widgets.ImageButton(new Point(373, 460), this.ubtn, this.dbtn));
		}

		public void Dispose()
		{
			if (!disposed)
			{
				foreach (var widget in widgets)
					widget.Dispose();

				back.Dispose();
				logo.Dispose();
				ubtn.Dispose();
				dbtn.Dispose();

				disposed = true;
			}
		}

		public void Draw(GOut g)
		{
			foreach (var widget in widgets)
			{
				if (widget.Visible)
				{
					g.PushMatrix();
					g.Translate(widget.Location);
					widget.Draw(g);
					g.PopMatrix();
				}
			}
		}

		public void OnButtonDown(MouseButtonEventArgs e)
		{
			var widget = GetWidgetAt(e.Position);
			if (widget != null)
				widget.OnButtonDown(e);
		}

		public void OnButtonUp(MouseButtonEventArgs e)
		{
			var widget = GetWidgetAt(e.Position);
			if (widget != null)
				widget.OnButtonUp(e);
		}

		private Widget GetWidgetAt(Point p)
		{
			for (int i = widgets.Count - 1; i >= 0; i--)
			{
				var widget = widgets[i];
				if (widget.Bounds.Contains(p.X, p.Y))
					return widget;
			}
			return null;
		}

	}
}

