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
			var background = new ImageWidget(RootWidget);
			background.Image = ResourceManager.LoadTexture("gfx/loginscr");
			background.SetLocation(0, 0);
			background.SetSize(background.Image.Width, background.Image.Height);

			var logo = new ImageWidget(RootWidget);
			logo.Image = ResourceManager.LoadTexture("gfx/logo");
			logo.SetLocation(420 - logo.Image.Width / 2, 215 - logo.Image.Height / 2);
			logo.SetSize(logo.Width, logo.Height);

			var btnLogin = new ImageButton(RootWidget);
			btnLogin.Up = ResourceManager.LoadTexture("gfx/hud/buttons/loginu");
			btnLogin.Down = ResourceManager.LoadTexture("gfx/hud/buttons/logind");
			btnLogin.Pressed += (sender, args) => OnLogin();
			btnLogin.SetLocation(373, 460);
			btnLogin.SetSize(btnLogin.Up.Width, btnLogin.Up.Height);

			var lbUserName = new Label(RootWidget);
			lbUserName.Text = "User Name";
			lbUserName.TextColor = Color.White;
			lbUserName.SetLocation(345, 310).SetSize(150, 20);

			var tbUserName = new TextBox(RootWidget);
			tbUserName.Text = "ken_tower";
			tbUserName.SetLocation(345, 330).SetSize(150, 20);

			var lbPassword = new Label(RootWidget);
			lbPassword.Text = "Password";
			lbPassword.TextColor = Color.White;
			lbPassword.SetLocation(345, 370).SetSize(150, 20);

			var tbPassword = new TextBox(RootWidget);
			tbPassword.Text = "pwd";
			tbPassword.SetLocation(345, 390).SetSize(150, 20);
		}

		protected override void OnResize(int newWidth, int newHeight)
		{
			var w = newWidth > MinWidth ? newWidth : MinWidth;
			var h = newHeight > MinHeight ? newHeight : MinHeight;
			RootWidget.SetLocation((w - MinWidth) / 2, (h - MinHeight) / 2);
		}

		private void OnLogin()
		{
			Host.SetScreen(new GameSessionScreen(Host));
		}
	}
}
