using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.UI;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace MonoHaven
{
	public sealed class MainWindow : GameWindow
	{
		private const string WindowTitle = "MonoHaven";

		private IScreen currentScreen;
		private readonly FrameCounter frameCounter;
		private readonly ConcurrentQueue<Action> updateQueue;
		private DateTime lastUpdate = DateTime.Now;

		public MainWindow(int width, int height)
			: base(width, height, GraphicsMode.Default, WindowTitle)
		{
			frameCounter = new FrameCounter();
			updateQueue = new ConcurrentQueue<Action>();

			VSync = VSyncMode.On;

			Mouse.ButtonUp += HandleMouseButtonUp;
			Mouse.ButtonDown += HandleMouseButtonDown;
			Mouse.Move += HandleMouseMove;
			Mouse.WheelChanged += HandleMouseWheel;

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

		public void QueueUpdate(Action action)
		{
			updateQueue.Enqueue(action);
		}

		public void SetCursor(MouseCursor cursor)
		{
			Cursor = cursor;
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

			currentScreen = new MainScreen();
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

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);

			var now = DateTime.Now;
			currentScreen.Update((now - lastUpdate).Milliseconds);
			lastUpdate = now;

			Action action;
			while (updateQueue.TryDequeue(out action))
				action();
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

#if DEBUG
			frameCounter.Update();
			Title = string.Format("{0} [FPS: {1}] [Render Calls: {2}]", WindowTitle, frameCounter.FramesPerSecond, SpriteBatch.RenderCount);
			SpriteBatch.RenderCount = 0;
#endif
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			currentScreen.Close();
		}

		private void HandleMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			currentScreen.MouseButtonDown(InputConverter.Map(e));
		}

		private void HandleMouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			currentScreen.MouseButtonUp(InputConverter.Map(e));
		}

		private void HandleMouseMove(object sedner, MouseMoveEventArgs e)
		{
			currentScreen.MouseMove(InputConverter.Map(e));
		}

		private void HandleMouseWheel(object sender, MouseWheelEventArgs e)
		{
			currentScreen.MouseWheel(InputConverter.Map(e));
		}

		private void HandleKeyDown(object sender, KeyboardKeyEventArgs e)
		{
			currentScreen.KeyDown(InputConverter.Map(e));
		}

		private void HandleKeyUp(object sender, KeyboardKeyEventArgs e)
		{
			currentScreen.KeyUp(InputConverter.Map(e));
		}

		private void HandleKeyPress(object sender, KeyPressEventArgs e)
		{
			currentScreen.KeyPress(InputConverter.Map(e));
		}
	}
}
