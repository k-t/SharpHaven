﻿using System.Drawing;
using MonoHaven.Resources;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI
{
	public class LoginScreen : BaseScreen
	{
		public LoginScreen(IScreenHost host)
			: base(host)
		{
			InitializeWidgets();
		}

		private void InitializeWidgets()
		{
			var background = ResourceManager.LoadTexture("gfx/loginscr");
			AddWidget(new ImageWidget {
				X = 0, Y = 0,
				Width = background.Width,
				Height = background.Height,
				Image = background
			});

			var logo = ResourceManager.LoadTexture("gfx/logo");
			AddWidget(new ImageWidget {
				X = 420 - logo.Width / 2,
				Y = 215 - logo.Height / 2,
				Width = logo.Width,
				Height = logo.Height,
				Image =  logo
			});

			var upTexture = ResourceManager.LoadTexture("gfx/hud/buttons/loginu");
			var btnLogin = new ImageButton {
				X = 373,
				Y = 460,
				Width = upTexture.Width,
				Height = upTexture.Height,
				Up = upTexture,
				Down = ResourceManager.LoadTexture("gfx/hud/buttons/logind")
			};
			btnLogin.Pressed += (sender, args) => OnLogin();
			AddWidget(btnLogin);

			AddWidget(new Label { Text = "User Name", TextColor = Color.White, X = 345, Y = 310 });
			AddWidget(new Label { Text = "Password", TextColor = Color.White, X = 345, Y = 370 });
		}

		private void OnLogin()
		{
			Host.CurrentScreen = new GameSessionScreen(Host);
		}
	}
}
