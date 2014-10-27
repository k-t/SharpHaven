using System.Drawing;
using MonoHaven.Resources;

namespace MonoHaven.UI
{
	public class LoginScreen : BaseScreen
	{
		private const int MinWidth = 800;
		private const int MinHeight = 600;

		public LoginScreen(IScreenHost host)
			: base(host)
		{
			InitializeWidgets();
		}

		private void InitializeWidgets()
		{
			var background = ResourceManager.LoadTexture("gfx/loginscr");
			Add(new ImageWidget {
				X = 0, Y = 0,
				Width = background.Width,
				Height = background.Height,
				Image = background
			});

			var logo = ResourceManager.LoadTexture("gfx/logo");
			Add(new ImageWidget {
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
			Add(btnLogin);

			Add(new Label { Text = "User Name", TextColor = Color.White, X = 345, Y = 310, Width = 150, Height = 20 });
			Add(new TextBox { X = 345, Y = 330, Width = 150, Height = 20, Text = "ken_tower" });
			Add(new Label { Text = "Password", TextColor = Color.White, X = 345, Y = 370, Width = 150, Height = 20 });
			Add(new TextBox { X = 345, Y = 390, Width = 150, Height = 20, Text = "pwd" });
		}

		protected override void OnResize(int newWidth, int newHeight)
		{
			var w = newWidth > MinWidth ? newWidth : MinWidth;
			var h = newHeight > MinHeight ? newHeight : MinHeight;
			RootWidget.Location = new Point((w - MinWidth) / 2, (h - MinHeight) / 2);
		}

		private void OnLogin()
		{
			Host.CurrentScreen = new GameSessionScreen(Host);
		}
	}
}
