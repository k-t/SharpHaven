using System;
using System.Text;
using MonoHaven.Input;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class BuddyWindow : Window
	{
		private BuddyList buddyList;
		private BuddyInfo buddyInfo;
		private TextBox tbSecret;
		private TextBox tbKinSecret;

		public BuddyWindow(Widget parent) : base(parent, "Kin")
		{
			buddyList = new BuddyList(this);
			buddyList.Move(10, 5);
			buddyList.Resize(180, 280);

			// TODO: get comparison function from settings
			buddyList.SetSort(BuddyComparison.ByStatus);

			buddyInfo = new BuddyInfo(this);
			buddyInfo.Move(210, 5);
			buddyInfo.Resize(180, 280);

			var btnStatus = new Button(this, 120);
			btnStatus.Text = "Sort by status";
			btnStatus.Move(5, 290);
			btnStatus.Click += () => buddyList.SetSort(BuddyComparison.ByStatus);

			var btnGroup = new Button(this, 120);
			btnGroup.Text = "Sort by group";
			btnGroup.Move(140, 290);
			btnGroup.Click += () => buddyList.SetSort(BuddyComparison.ByGroup);

			var btnAlpha = new Button(this, 120);
			btnAlpha.Text = "Sort alphabetically";
			btnAlpha.Move(275, 290);
			btnAlpha.Click += () => buddyList.SetSort(BuddyComparison.Alphabetical);

			var lblSecret = new Label(this, Fonts.LabelText);
			lblSecret.Move(0, 310);
			lblSecret.Text = "My hearth secret:";

			var lblMakeKin = new Label(this, Fonts.LabelText);
			lblMakeKin.Move(200, 310);
			lblMakeKin.Text = "Make kin by hearth secret:";

			tbSecret = new TextBox(this);
			tbSecret.Move(0, 326);
			tbSecret.Resize(190, 20);
			tbSecret.KeyDown += OnSecretTextBoxKeyDown;

			tbKinSecret = new TextBox(this);
			tbKinSecret.Move(200, 325);
			tbKinSecret.Resize(190, 20);
			tbKinSecret.KeyDown += OnKinSecretTextBoxKeyDown;

			var btnSetSecret = new Button(this, 50);
			btnSetSecret.Text = "Set";
			btnSetSecret.Move(0, 350);
			btnSetSecret.Click += OnSetSecretButtonClick;

			var btnClearSecret = new Button(this, 50);
			btnClearSecret.Text = "Clear";
			btnClearSecret.Move(60, 350);
			btnClearSecret.Click += OnClearSecretButtonClick;

			var btnRandomSecret = new Button(this, 50);
			btnRandomSecret.Text = "Random";
			btnRandomSecret.Move(120, 350);
			btnRandomSecret.Click += OnRandomSecretButtonClick;

			var btnAddKin = new Button(this, 50);
			btnAddKin.Text = "Add kin";
			btnAddKin.Move(200, 350);
			btnAddKin.Click += () => AddKin.Raise(tbKinSecret.Text);

			// clear text box when kin is added
			AddKin += _ => tbKinSecret.Text = "";

			Pack();
		}

		public event Action ChangeSecret;
		public event Action<string> AddKin;

		public BuddyInfo BuddyInfo
		{
			get { return buddyInfo; }
		}

		public BuddyList BuddyList
		{
			get { return buddyList; }
		}

		public string Secret
		{
			get { return tbSecret.Text; }
			set { tbSecret.Text = value; }
		}

		private void OnClearSecretButtonClick()
		{
			Secret = "";
			ChangeSecret.Raise();
		}

		private void OnSetSecretButtonClick()
		{
			ChangeSecret.Raise();
		}

		private void OnRandomSecretButtonClick()
		{
			Secret = GenerateRandomSecret();
			ChangeSecret.Raise();
		}

		private void OnSecretTextBoxKeyDown(KeyEvent args)
		{
			if (args.Key == Key.Enter)
				ChangeSecret.Raise();
		}

		private void OnKinSecretTextBoxKeyDown(KeyEvent args)
		{
			if (args.Key == Key.Enter)
				AddKin.Raise(tbKinSecret.Text);
		}

		private static string GenerateRandomSecret()
		{
			var random = new Random((int)DateTime.Now.Ticks);
			var charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var buf = new StringBuilder();
			for (int i = 0; i < 8; i++)
				buf.Append(charset[random.Next(charset.Length)]);
			return buf.ToString();
		}
	}
}
