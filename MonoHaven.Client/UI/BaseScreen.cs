using System;
using MonoHaven.Graphics;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public abstract class BaseScreen : IDisposable, IScreen
	{
		private readonly IScreenHost host;
		private readonly RootWidget rootWidget;

		protected BaseScreen(IScreenHost host)
		{
			this.host = host;
			this.rootWidget = new RootWidget();
		}

		protected IScreenHost Host
		{
			get { return host; }
		}

		protected RootWidget RootWidget
		{
			get { return rootWidget; }
		}

		protected virtual void OnShow() { }
		protected virtual void OnClose() { }
		protected virtual void OnResize(int newWidth, int newHeight) { }

		protected virtual void OnDraw(DrawingContext g)
		{
			rootWidget.Draw(g);
		}

		protected virtual void OnMouseButtonDown(MouseButtonEventArgs e)
		{
			var widget = rootWidget.GetChildAt(e.Position);
			if (widget != null)
				widget.OnButtonDown(e);
		}

		protected virtual void OnMouseButtonUp(MouseButtonEventArgs e)
		{
			var widget = rootWidget.GetChildAt(e.Position);
			if (widget != null)
				widget.OnButtonUp(e);
		}

		protected virtual void OnMouseMove(MouseMoveEventArgs e)
		{
			var widget = rootWidget.GetChildAt(e.Position);
			if (widget != null)
				widget.OnMouseMove(e);
		}

		protected virtual void OnKeyDown(KeyboardKeyEventArgs e)
		{
			// TODO: focused widget
			rootWidget.OnKeyDown(e);
		}

		protected virtual void OnKeyUp(KeyboardKeyEventArgs e)
		{
			rootWidget.OnKeyUp(e);
		}

		protected void Add(Widget widget)
		{
			rootWidget.Add(widget);
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

		void IScreen.Draw(DrawingContext drawingContext)
		{
			this.OnDraw(drawingContext);
		}

		void IScreen.HandleMouseButtonDown(MouseButtonEventArgs e)
		{
			this.OnMouseButtonDown(e);
		}

		void IScreen.HandleMouseButtonUp(MouseButtonEventArgs e)
		{
			this.OnMouseButtonUp(e);
		}

		void IScreen.HandleMouseMove(MouseMoveEventArgs e)
		{
			this.OnMouseMove(e);
		}

		void IScreen.HandleKeyDown(KeyboardKeyEventArgs e)
		{
			this.OnKeyDown(e);
		}

		void IScreen.HandleKeyUp(KeyboardKeyEventArgs e)
		{
			this.OnKeyUp(e);
		}

		#endregion
	}
}
