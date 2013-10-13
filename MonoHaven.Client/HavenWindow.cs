using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MonoHaven
{
	public sealed class HavenWindow : GameWindow
	{	
		private UI ui;

		public HavenWindow(int width, int height)
			: base(width, height, GraphicsMode.Default, "MonoHaven")
		{
			this.VSync = VSyncMode.On;

			this.Load += HandleLoad;
			this.Resize += HandleResize;
			this.UpdateFrame += HandleUpdateFrame;

			this.Mouse.ButtonUp += HandleButtonUp;
			this.Mouse.ButtonDown += HandleButtonDown;
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

			this.ui = new UI();
		}

		private void HandleResize(object sender, EventArgs e)
		{
			GL.Viewport(0, 0, Width, Height);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, Width, Height, 0, -1, 1);
		}

		private void HandleUpdateFrame (object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			var g = new GOut();
			ui.Draw(g);

			SwapBuffers();
		}

		private void HandleButtonUp(object sender, MouseButtonEventArgs e)
		{
			ui.OnButtonUp(e);
		}

		private void HandleButtonDown(object sender, MouseButtonEventArgs e)
		{
			ui.OnButtonDown(e);
		}
	}
}
