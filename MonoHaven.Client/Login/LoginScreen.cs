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

		private Login login;

		private Container cntPassword;
		private TextBox tbUserName;
		private TextBox tbPassword;
		private Label lblErrorMessage;

		private Container cntToken;
		private Label lblUserName;
		private Button btnForget;

		private Label lbProgress;
		private ImageButton btnLogin;

		public LoginScreen()
		{
			login = new Login();
			InitWidgets();
			GotoInitialPage();
			RootWidget.KeyDown += OnKeyDown;
		}

		public event Action<GameSession> LoginCompleted;

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

			cntPassword = new Container(RootWidget);
			cntPassword.Move(345, 310).Resize(150, 100);

			var lbUserName = new Label(cntPassword);
			lbUserName.Text = "User Name";
			lbUserName.TextColor = Color.White;
			lbUserName.Move(0, 0).Resize(150, 20);

			tbUserName = new TextBox(cntPassword);
			tbUserName.Text = "ken_tower";
			tbUserName.Move(0, 20).Resize(150, 23);

			var lbPassword = new Label(cntPassword);
			lbPassword.Text = "Password";
			lbPassword.TextColor = Color.White;
			lbPassword.Move(0, 60).Resize(150, 20);

			tbPassword = new TextBox(cntPassword);
			tbPassword.PasswordChar = '*';
			tbPassword.Move(0, 80).Resize(150, 23);

			cntToken = new Container(RootWidget);
			cntToken.Move(295, 310).Resize(250, 100);

			lblUserName = new Label(cntToken);
			lblUserName.Resize(cntToken.Width, cntToken.Height);
			lblUserName.TextAlign = TextAlign.Center;
			lblUserName.Text = string.Format("Identity is saved for {0}", login.UserName);

			btnForget = new Button(cntToken, 100);
			btnForget.Text = "Forget me";
			btnForget.Move(75, 23);
			btnForget.Clicked += ForgetToken;

			lblErrorMessage = new Label(RootWidget);
			lblErrorMessage.TextColor = Color.Red;
			lblErrorMessage.TextAlign = TextAlign.Center;
			lblErrorMessage.Visible = false;
			lblErrorMessage.Move(0, 500).Resize(840, 20);

			lbProgress = new Label(RootWidget);
			lbProgress.TextColor = Color.White;
			lbProgress.TextAlign = TextAlign.Center;
			lbProgress.Visible = false;
			lbProgress.Move(0, 350).Resize(840, 20);
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
					if (string.IsNullOrEmpty(tbUserName.Text))
						SetKeyboardFocus(tbUserName);
					else if (tbUserName.IsFocused || string.IsNullOrEmpty(tbPassword.Text))
						SetKeyboardFocus(tbPassword);
					else
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
			GotoProgressPage();

			if (!login.HasToken)
			{
				login.UserName = tbUserName.Text;
				login.Password = tbPassword.Text;
			}

			var authResult = await login.LoginAsync(UpdateProgress);
			if (authResult.IsSuccessful)
			{
				LoginCompleted.Raise(authResult.Session);
				GotoInitialPage();
			}
			else
				GotoInitialPage(authResult.Error);
		}

		private void GotoInitialPage(string errorMessage = null)
		{
			lbProgress.Visible = false;
			
			cntPassword.Visible = !login.HasToken;
			cntToken.Visible = login.HasToken;
			btnLogin.Visible = true;

			lblErrorMessage.Visible = !string.IsNullOrEmpty(errorMessage);
			lblErrorMessage.Text = errorMessage;

			tbUserName.Text = login.UserName;
			if (string.IsNullOrEmpty(tbUserName.Text))
				SetKeyboardFocus(tbUserName);
			else
				SetKeyboardFocus(tbPassword);
		}

		private void GotoProgressPage()
		{
			lbProgress.Text = "";
			lbProgress.Visible = true;

			cntPassword.Visible = false;
			cntToken.Visible = false;
			btnLogin.Visible = false;
			
			lblErrorMessage.Visible = false;
		}

		private void UpdateProgress(string status)
		{
			lbProgress.Text = status;
		}

		private void ForgetToken()
		{
			login.ForgetToken();
			GotoInitialPage();
		}
	}
}
