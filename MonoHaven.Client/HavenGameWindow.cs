using System;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.UI;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MonoHaven
{
	public sealed class HavenGameWindow : GameWindow, IScreenHost
	{
		private const string WindowTitle = "MonoHaven";

		private readonly ScreenManager screenManager;

		public HavenGameWindow(int width, int height)
			: base(width, height, GraphicsMode.Default, WindowTitle)
		{
			this.VSync = VSyncMode.On;

			this.Load += HandleLoad;
			this.Resize += HandleResize;
			this.UpdateFrame += HandleUpdateFrame;
			this.Unload += HandleUnload;

			this.Mouse.ButtonUp += HandleButtonUp;
			this.Mouse.ButtonDown += HandleButtonDown;
			this.Mouse.Move += HandleMouseMove;
			this.Keyboard.KeyDown += HandleKeyDown;
			this.Keyboard.KeyUp += HandleKeyUp;
			this.Keyboard.KeyRepeat = true;

			this.screenManager = new ScreenManager();
		}

		public event EventHandler Resized;

		private void HandleLoad(object sender, EventArgs e)
		{
			GL.Color3(Color.White);
			GL.PointSize(4);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.Texture2D);
			GL.Disable(EnableCap.DepthTest);
			GL.Disable(EnableCap.Lighting);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha,
			             BlendingFactorDest.OneMinusSrcAlpha);

			this.screenManager.Init(this);
		}

		private void HandleUnload(object sender, EventArgs e)
		{
			this.screenManager.Dispose();
		}

		private void HandleResize(object sender, EventArgs e)
		{
			GL.Viewport(0, 0, Width, Height);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, Width, Height, 0, -1, 1);
			RaiseResizedEvent();
		}

		private void HandleUpdateFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			this.screenManager.CurrentScreen.Draw(new DrawingContext());

			Title = string.Format("{0}: {1} FPS", WindowTitle, (int)UpdateFrequency);

			SwapBuffers();
		}

		private void HandleButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.screenManager.CurrentScreen.OnButtonUp(e);
		}

		private void HandleButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.screenManager.CurrentScreen.OnButtonDown(e);
		}

		private void HandleKeyDown(object sender, KeyboardKeyEventArgs e)
		{
			this.screenManager.CurrentScreen.OnKeyDown(e);
		}

		private void HandleKeyUp(object sender, KeyboardKeyEventArgs e)
		{
			this.screenManager.CurrentScreen.OnKeyUp(e);
		}

		private void HandleMouseMove(object sedner, MouseMoveEventArgs e)
		{
			this.screenManager.CurrentScreen.OnMouseMove(e);
		}

		private void RaiseResizedEvent()
		{
			if (Resized != null)
				Resized(this, EventArgs.Empty);
		}
	}
}
