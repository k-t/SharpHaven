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

		public MainWindow(int width, int height)
			: base(width, height, GraphicsMode.Default, WindowTitle)
		{
			this.currentScreen = EmptyScreen.Instance;

			this.VSync = VSyncMode.On;

			this.Mouse.ButtonUp += HandleMouseButtonUp;
			this.Mouse.ButtonDown += HandleMouseButtonDown;
			this.Mouse.Move += HandleMouseMove;

			this.Keyboard.KeyRepeat = true;
			this.Keyboard.KeyDown += HandleKeyDown;
			this.Keyboard.KeyUp += HandleKeyUp;
		}

		public IScreen CurrentScreen
		{
			get { return currentScreen; }
			set
			{
				currentScreen.Close();
				currentScreen = value ?? EmptyScreen.Instance;
				currentScreen.Show();
			}
		}

		private SpriteBatch SpriteBatch { get; set; }

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		
			GL.Color3(Color.White);
			GL.PointSize(4);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.Texture2D);
			GL.Disable(EnableCap.DepthTest);
			GL.Disable(EnableCap.Lighting);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha,
			             BlendingFactorDest.OneMinusSrcAlpha);

			CurrentScreen = new LoginScreen(this);
			SpriteBatch = new SpriteBatch();
		}

		protected override void OnUnload(EventArgs e)
		{
			base.OnUnload(e);
			CurrentScreen.Close();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
		
			GL.Viewport(0, 0, Width, Height);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, Width, Height, 0, -1, 1);

			CurrentScreen.Resize(Width, Height);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			GL.Clear(ClearBufferMask.ColorBufferBit);
			using (var dc = new DrawingContext(SpriteBatch))
			{
				CurrentScreen.Draw(dc);
			}
			SwapBuffers();

			Title = string.Format("{0}: {1} FPS", WindowTitle, FpsCounter.GetFps());
		}

		private void HandleMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			CurrentScreen.HandleMouseButtonDown(e);
		}

		private void HandleMouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			CurrentScreen.HandleMouseButtonUp(e);
		}

		private void HandleMouseMove(object sedner, MouseMoveEventArgs e)
		{
			CurrentScreen.HandleMouseMove(e);
		}

		private void HandleKeyDown(object sender, KeyboardKeyEventArgs e)
		{
			CurrentScreen.HandleKeyDown(e);
		}

		private void HandleKeyUp(object sender, KeyboardKeyEventArgs e)
		{
			CurrentScreen.HandleKeyUp(e);
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
