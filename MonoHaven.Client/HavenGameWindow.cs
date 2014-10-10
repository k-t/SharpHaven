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
	public sealed class HavenGameWindow : GameWindow
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

			this.screenManager = new ScreenManager();
		}

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

			this.screenManager.Init();
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
	}
}
