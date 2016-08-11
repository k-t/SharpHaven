using System;
using Haven.Net;
using SharpHaven.Client;
using SharpHaven.Graphics;
using SharpHaven.Input;
using SharpHaven.Login;
using SharpHaven.UI;

namespace SharpHaven
{
	public class MainScreen : IScreen
	{
		private readonly LoginScreen loginScreen;
		private GameScreen gameScreen;
		private IScreen current;
		private GameClient client;
		private ClientSession session;

		public MainScreen()
		{
			var clientConfig = new GameClientConfiguration
			{
				AuthServerAddress = new NetworkAddress(App.Config.AuthHost, App.Config.AuthPort),
				GameServerAddress = new NetworkAddress(App.Config.GameHost, App.Config.GamePort)
			};

			client = new GameClient(clientConfig);
			loginScreen = new LoginScreen(client);
			loginScreen.LoginCompleted += OnLoginCompleted;
			current = loginScreen;
		}

		private void ChangeScreen(IScreen screen)
		{
			if (screen == null)
				throw new ArgumentNullException(nameof(screen));

			current.Close();
			current = screen;
			current.Resize(App.Window.ClientSize);
			current.Show();
		}

		private void OnLoginCompleted()
		{
			session = new ClientSession(client);
			session.Start();

			gameScreen = session.Screen;
			gameScreen.Closed += OnGameExited;

			ChangeScreen(gameScreen);
		}

		private void OnGameExited()
		{
			gameScreen.Closed -= OnGameExited;
			session.Finish();

			ChangeScreen(loginScreen);

			gameScreen.Dispose();
			// ensure that the instance is not lingering
			gameScreen = null;
			session = null;
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

			if (session != null)
			{
				session.Objects.Tick(dt);
				session.Scene.Update();
			}
		}

		void IScreen.MouseButtonDown(MouseButtonEvent e)
		{
			current.MouseButtonDown(e);
		}

		void IScreen.MouseButtonUp(MouseButtonEvent e)
		{
			current.MouseButtonUp(e);
		}

		void IScreen.MouseMove(MouseMoveEvent e)
		{
			current.MouseMove(e);
		}

		void IScreen.MouseWheel(MouseWheelEvent e)
		{
			current.MouseWheel(e);
		}

		void IScreen.KeyDown(KeyEvent e)
		{
			current.KeyDown(e);
		}

		void IScreen.KeyUp(KeyEvent e)
		{
			current.KeyUp(e);
		}

		void IScreen.KeyPress(KeyPressEvent e)
		{
			current.KeyPress(e);
		}

		#endregion
	}
}
