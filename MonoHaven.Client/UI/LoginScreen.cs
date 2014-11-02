using System.Drawing;
using System.Threading.Tasks;
using MonoHaven.Graphics;
using MonoHaven.Network;
using MonoHaven.Resources;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public class LoginScreen : BaseScreen
	{
		private const int MinWidth = 800;
		private const int MinHeight = 600;

		private LoginService loginService;

		private WidgetGroup grLogin;
		private TextBox tbUserName;
		private TextBox tbPassword;
		private Label lbErrorMessage;
		private Label lbProgress;
		private ImageButton btnLogin;

		public LoginScreen(IScreenHost host)
			: base(host)
		{
			InitializeAuthClient();
			InitializeWidgets();
		}

		private void InitializeAuthClient()
		{
			loginService = new LoginService(Config.LoginOptions);
			loginService.StatusChanged += HandleLoginStatusChange;
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

			btnLogin = new ImageButton(RootWidget);
			btnLogin.Up = ResourceManager.LoadTexture("gfx/hud/buttons/loginu");
			btnLogin.Down = ResourceManager.LoadTexture("gfx/hud/buttons/logind");
			btnLogin.Pressed += (sender, args) => Login();
			btnLogin.SetLocation(373, 460);
			btnLogin.SetSize(btnLogin.Up.Width, btnLogin.Up.Height);

			grLogin = new WidgetGroup(RootWidget);
			grLogin.SetLocation(345, 310).SetSize(150, 100);

			var lbUserName = new Label(grLogin);
			lbUserName.Text = "User Name";
			lbUserName.TextColor = Color.White;
			lbUserName.SetLocation(0, 0).SetSize(150, 20);

			tbUserName = new TextBox(grLogin);
			tbUserName.Text = "ken_tower";
			tbUserName.SetLocation(0, 20).SetSize(150, 23);

			var lbPassword = new Label(grLogin);
			lbPassword.Text = "Password";
			lbPassword.TextColor = Color.White;
			lbPassword.SetLocation(0, 60).SetSize(150, 20);

			tbPassword = new TextBox(grLogin);
			tbPassword.Text = "pwd";
			tbPassword.PasswordChar = '*';
			tbPassword.SetLocation(0, 80).SetSize(150, 23);

			lbErrorMessage = new Label(RootWidget);
			lbErrorMessage.TextColor = Color.Red;
			lbErrorMessage.TextAlign = TextAlign.Center;
			lbErrorMessage.Visible = false;
			lbErrorMessage.SetLocation(0, 500).SetSize(840, 20);

			lbProgress = new Label(RootWidget);
			lbProgress.TextColor = Color.White;
			lbProgress.TextAlign = TextAlign.Center;
			lbProgress.Visible = false;
			lbProgress.SetLocation(0, 350).SetSize(840, 20);

			SetKeyboardFocus(tbUserName);
		}

		protected override void OnResize(int newWidth, int newHeight)
		{
			var w = newWidth > MinWidth ? newWidth : MinWidth;
			var h = newHeight > MinHeight ? newHeight : MinHeight;
			RootWidget.SetLocation((w - MinWidth) / 2, (h - MinHeight) / 2);
		}

		protected override void OnKeyDown(KeyEventArgs args)
		{
			switch (args.Key)
			{
				case Key.Enter:
					Login();
					break;
				case Key.Tab:
					if (tbPassword.IsFocused)
						SetKeyboardFocus(tbUserName);
					else if (tbUserName.IsFocused)
						SetKeyboardFocus(tbPassword);
					break;
			}
		}

		private async void Login()
		{
			btnLogin.Visible = false;
			grLogin.Visible = false;
			lbErrorMessage.Visible = false;
			lbProgress.Text = "";
			lbProgress.Visible = true;

			var authResult = await loginService.LoginAsync(tbUserName.Text, tbPassword.Text);

			lbProgress.Visible = false;
			grLogin.Visible = true;
			btnLogin.Visible = true;

			if (authResult.IsSuccessful)
			{
				Host.SetScreen(new GameSessionScreen(Host));
			}
			else
			{
				lbErrorMessage.Visible = true;
				lbErrorMessage.Text = authResult.Error;
			}
		}

		private void HandleLoginStatusChange(object sender, LoginStatusEventArgs args)
		{
			lbProgress.Text = args.Status;
		}
	}
}
