using System;
using System.Drawing;
using System.Threading.Tasks;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.UI;
using MonoHaven.UI.Widgets;
using OpenTK.Input;
using Image = MonoHaven.UI.Widgets.Image;

namespace MonoHaven.Login
{
	public class LoginScreen : BaseScreen
	{
		private const int MinWidth = 800;
		private const int MinHeight = 600;

		private LoginService loginService;

		private Container grLogin;
		private TextBox tbUserName;
		private TextBox tbPassword;
		private Label lbErrorMessage;
		private Label lbProgress;
		private ImageButton btnLogin;

		public LoginScreen()
		{
			InitAuthClient();
			InitWidgets();

			RootWidget.KeyDown += OnKeyDown;
		}

		public event Action<GameSession> LoginCompleted;

		private void InitAuthClient()
		{
			loginService = new LoginService();
		}

		private void InitWidgets()
		{
			var background = new Image(RootWidget);
			background.Drawable = App.Resources.Get<Drawable>("gfx/loginscr");
			background.Move(0, 0);
			background.Resize(background.Drawable.Size);

			var logo = new Image(RootWidget);
			logo.Drawable = App.Resources.Get<Drawable>("gfx/logo");
			logo.Move(420 - logo.Drawable.Width / 2, 215 - logo.Drawable.Height / 2);
			logo.Resize(logo.Size);

			btnLogin = new ImageButton(RootWidget);
			btnLogin.Image = App.Resources.Get<Drawable>("gfx/hud/buttons/loginu");
			btnLogin.PressedImage = App.Resources.Get<Drawable>("gfx/hud/buttons/logind");
			btnLogin.Clicked += Login;
			btnLogin.Move(373, 460);
			btnLogin.Resize(btnLogin.Image.Size);

			grLogin = new Container(RootWidget);
			grLogin.Move(345, 310).Resize(150, 100);

			var lbUserName = new Label(grLogin);
			lbUserName.Text = "User Name";
			lbUserName.TextColor = Color.White;
			lbUserName.Move(0, 0).Resize(150, 20);

			tbUserName = new TextBox(grLogin);
			tbUserName.Text = "ken_tower";
			tbUserName.Move(0, 20).Resize(150, 23);

			var lbPassword = new Label(grLogin);
			lbPassword.Text = "Password";
			lbPassword.TextColor = Color.White;
			lbPassword.Move(0, 60).Resize(150, 20);

			tbPassword = new TextBox(grLogin);
			tbPassword.Text = "pwd";
			tbPassword.PasswordChar = '*';
			tbPassword.Move(0, 80).Resize(150, 23);

			lbErrorMessage = new Label(RootWidget);
			lbErrorMessage.TextColor = Color.Red;
			lbErrorMessage.TextAlign = TextAlign.Center;
			lbErrorMessage.Visible = false;
			lbErrorMessage.Move(0, 500).Resize(840, 20);

			lbProgress = new Label(RootWidget);
			lbProgress.TextColor = Color.White;
			lbProgress.TextAlign = TextAlign.Center;
			lbProgress.Visible = false;
			lbProgress.Move(0, 350).Resize(840, 20);

			SetKeyboardFocus(tbUserName);
		}

		protected override void OnResize(int newWidth, int newHeight)
		{
			var w = newWidth > MinWidth ? newWidth : MinWidth;
			var h = newHeight > MinHeight ? newHeight : MinHeight;
			RootWidget.Move((w - MinWidth) / 2, (h - MinHeight) / 2);
		}

		private void OnKeyDown(KeyEvent args)
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

			var authResult = await loginService.LoginAsync(
				tbUserName.Text, tbPassword.Text, ChangeProgress);

			if (authResult.IsSuccessful)
			{
				LoginCompleted.Raise(authResult.Session);
			}
			else
			{
				lbProgress.Visible = false;
				grLogin.Visible = true;
				btnLogin.Visible = true;

				lbErrorMessage.Visible = true;
				lbErrorMessage.Text = authResult.Error;
			}
		}

		private void ChangeProgress(string status)
		{
			lbProgress.Text = status;
		}
	}
}
