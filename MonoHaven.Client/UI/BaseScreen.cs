using System;
using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public abstract class BaseScreen : IDisposable, IScreen
	{
		private readonly IScreenHost host;
		private readonly RootWidget rootWidget;

		protected BaseScreen(IScreenHost host)
		{
			this.rootWidget = new RootWidget();
			this.host = host;
			this.host.SetInputListener(rootWidget);
		}

		protected IScreenHost Host
		{
			get { return host; }
		}

		protected RootWidget RootWidget
		{
			get { return rootWidget; }
		}

		protected virtual void OnShow() {}

		protected virtual void OnClose() {}

		protected virtual void OnResize(int newWidth, int newHeight) {}

		protected virtual void OnDraw(DrawingContext dc)
		{
			rootWidget.Draw(dc);
		}

		protected Widget Add(Widget widget)
		{
			rootWidget.AddChild(widget);
			return widget;
		}

		public virtual void Dispose()
		{
			rootWidget.Dispose();
		}

		#region IScreen implementation

		void IScreen.Show()
		{
			this.OnShow();
		}

		void IScreen.Close()
		{
			this.OnClose();
		}

		void IScreen.Resize(int newWidth, int newHeight)
		{
			this.OnResize(newWidth, newHeight);
		}

		void IScreen.Draw(DrawingContext dc)
		{
			this.OnDraw(dc);
		}

		#endregion
	}
}
