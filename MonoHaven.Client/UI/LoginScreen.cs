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
			var background = new ImageWidget();
			background.Image = ResourceManager.LoadTexture("gfx/loginscr");
			Add(background)
				.SetLocation(0, 0)
				.SetSize(background.Image.Width, background.Image.Height);

			var logo = new ImageWidget();
			logo.Image = ResourceManager.LoadTexture("gfx/logo");
			Add(logo)
				.SetLocation(420 - logo.Image.Width / 2, 215 - logo.Image.Height / 2)
				.SetSize(logo.Width, logo.Height);

			var btnLogin = new ImageButton();
			btnLogin.Up = ResourceManager.LoadTexture("gfx/hud/buttons/loginu");
			btnLogin.Down = ResourceManager.LoadTexture("gfx/hud/buttons/logind");
			btnLogin.Pressed += (sender, args) => OnLogin();
			Add(btnLogin)
				.SetLocation(373, 460)
				.SetSize(btnLogin.Up.Width, btnLogin.Up.Height);

			var lbUserName = new Label();
			lbUserName.Text = "User Name";
			lbUserName.TextColor = Color.White;
			Add(lbUserName).SetLocation(345, 310).SetSize(150, 20);

			var tbUserName = new TextBox();
			tbUserName.Text = "ken_tower";
			Add(tbUserName).SetLocation(345, 330).SetSize(150, 20);

			var lbPassword = new Label();
			lbPassword.Text = "Password";
			lbPassword.TextColor = Color.White;
			Add(lbPassword).SetLocation(345, 370).SetSize(150, 20);

			var tbPassword = new TextBox();
			tbPassword.Text = "pwd";
			Add(tbPassword).SetLocation(345, 390).SetSize(150, 20);
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
