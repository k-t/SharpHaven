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
	public sealed class MainWindow : GameWindow, IScreenHost
	{
		private const string WindowTitle = "MonoHaven";

		private IScreen currentScreen;
		private IInputListener inputListener;

		public MainWindow(int width, int height)
			: base(width, height, GraphicsMode.Default, WindowTitle)
		{
			currentScreen = EmptyScreen.Instance;

			VSync = VSyncMode.On;

			Mouse.ButtonUp += HandleMouseButtonUp;
			Mouse.ButtonDown += HandleMouseButtonDown;
			Mouse.Move += HandleMouseMove;

			Keyboard.KeyRepeat = true;
			Keyboard.KeyDown += HandleKeyDown;
			Keyboard.KeyUp += HandleKeyUp;
			KeyPress += HandleKeyPress;
		}

		private SpriteBatch SpriteBatch
		{
			get;
			set;
		}

		public void SetCursor(MouseCursor cursor)
		{
			Cursor = cursor;
		}

		public void SetScreen(IScreen screen)
		{
			currentScreen.Close();
			currentScreen = screen ?? EmptyScreen.Instance;
			inputListener = currentScreen;
			currentScreen.Resize(Width, Height);
			currentScreen.Show();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		
			GL.Color3(Color.White);
			GL.PointSize(4);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.ScissorTest);
			GL.Disable(EnableCap.DepthTest);
			GL.Disable(EnableCap.Lighting);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			SetScreen(new LoginScreen(this));
			SpriteBatch = new SpriteBatch();
		}

		protected override void OnUnload(EventArgs e)
		{
			base.OnUnload(e);
			currentScreen.Close();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
		
			GL.Viewport(0, 0, Width, Height);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, Width, Height, 0, -1, 1);
			GL.Scissor(0, 0, Width, Height);

			currentScreen.Resize(Width, Height);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			GL.Clear(ClearBufferMask.ColorBufferBit);
			using (var dc = new DrawingContext(this, SpriteBatch))
			{
				currentScreen.Draw(dc);
			}
			SwapBuffers();

			Title = string.Format("{0} [FPS: {1}]", WindowTitle, FpsCounter.GetFps());
		}

		private void HandleMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (inputListener != null)
				inputListener.MouseButtonDown(e);
		}

		private void HandleMouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (inputListener != null)
				inputListener.MouseButtonUp(e);
		}

		private void HandleMouseMove(object sedner, MouseMoveEventArgs e)
		{
			if (inputListener != null)
				inputListener.MouseMove(e);
		}

		private void HandleKeyDown(object sender, KeyboardKeyEventArgs e)
		{
			if (inputListener != null)
				inputListener.KeyDown(new KeyEventArgs(e));
		}

		private void HandleKeyUp(object sender, KeyboardKeyEventArgs e)
		{
			if (inputListener != null)
				inputListener.KeyUp(new KeyEventArgs(e));
		}

		private void HandleKeyPress(object sender, OpenTK.KeyPressEventArgs e)
		{
			if (inputListener != null)
				inputListener.KeyPress(new UI.KeyPressEventArgs(e.KeyChar));
		}

		private static class FpsCounter
		{
			private static int fps;
			private static int frameCount;
			private static DateTime last;

			public static int GetFps()
			{
				frameCount++;
				var now = DateTime.Now;
				if ((now - last).TotalMilliseconds > 1000)
				{
					fps = frameCount;
					frameCount = 0;
					last = now;
				}
				return fps;
			}
		}
	}
}
