using System;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI
{
	public class LoginScreen : Screen
	{
		public LoginScreen(IScreenHost host)
			: base(host)
		{
			InitializeWidgets();
		}

		public event EventHandler LoggedIn;

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
			btnLogin.Pressed += (sender, args) => RaiseLoggedInEvent();
			AddWidget(btnLogin);
		}

		private void RaiseLoggedInEvent()
		{
			if (LoggedIn != null)
				LoggedIn(this, EventArgs.Empty);
		}
	}
}
