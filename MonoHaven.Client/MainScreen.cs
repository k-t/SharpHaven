using System;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Login;
using MonoHaven.UI;
using OpenTK;
using OpenTK.Input;

namespace MonoHaven
{
	public class MainScreen : IScreen
	{
		private readonly LoginScreen loginScreen;
		private GameScreen gameScreen;
		private IScreen current;

		public MainScreen()
		{
			loginScreen = new LoginScreen();
			loginScreen.LoginCompleted += OnLoginCompleted;
			current = loginScreen;
		}

		private void ChangeScreen(IScreen screen)
		{
			if (screen == null)
				throw new ArgumentNullException("screen");

			current.Close();
			current = screen;
			current.Resize(App.Window.Size);
			current.Show();
		}

		private void OnLoginCompleted(GameSession session)
		{
			gameScreen = session.Screen;
			gameScreen.Exited += OnGameExited;
			ChangeScreen(gameScreen);
		}

		private void OnGameExited()
		{
			gameScreen.Exited -= OnGameExited;

			ChangeScreen(loginScreen);

			gameScreen.Dispose();
			// ensure that the instance is not lingering
			gameScreen = null;
		}

		#region IScreen Implementation

		void IScreen.Show()
		{
			current.Show();
		}

		void IScreen.Close()
		{
			current.Close();
		}

		void IScreen.Resize(int newWidth, int newHeight)
		{
			current.Resize(newWidth, newHeight);
		}

		void IScreen.Draw(DrawingContext dc)
		{
			current.Draw(dc);
		}

		void IScreen.Update(int dt)
		{
			current.Update(dt);
		}

		void IScreen.MouseButtonDown(MouseButtonEventArgs e)
		{
			current.MouseButtonDown(e);
		}

		void IScreen.MouseButtonUp(MouseButtonEventArgs e)
		{
			current.MouseButtonUp(e);
		}

		void IScreen.MouseMove(MouseMoveEventArgs e)
		{
			current.MouseMove(e);
		}

		void IScreen.MouseWheel(MouseWheelEventArgs e)
		{
			current.MouseWheel(e);
		}

		void IScreen.KeyDown(KeyboardKeyEventArgs e)
		{
			current.KeyDown(e);
		}

		void IScreen.KeyUp(KeyboardKeyEventArgs e)
		{
			current.KeyUp(e);
		}

		void IScreen.KeyPress(KeyPressEventArgs e)
		{
			current.KeyPress(e);
		}

		#endregion
	}
}
