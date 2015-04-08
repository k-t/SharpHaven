using System;
using MonoHaven.Graphics;
using MonoHaven.Input;

namespace MonoHaven.UI.Widgets
{
	public class CombatRelation : Widget
	{
		private static readonly Drawable background;

		static CombatRelation()
		{
			background = App.Resources.Get<Drawable>("gfx/hud/bosq");
		}

		private readonly int id;
		private readonly AvatarView avatar;
		private readonly GiveButton button;
		private readonly Label label;

		private int balance;
		private int intensity;
		private int initiative;

		public CombatRelation(Widget parent, int id)
			: base(parent)
		{
			this.id = id;
			Resize(background.Size);

			avatar = new AvatarView(this);
			avatar.Resize(27, 27);
			avatar.Move(25, (Height - avatar.Height) / 2);
			avatar.Click += OnAvatarClick;

			button = new GiveButton(this);
			button.Resize(15, 15);
			button.Move(5, 4);
			button.Click += OnButtonClick;

			label = new Label(this, Fonts.Default);
			label.Move(65, 10);
		}

		public event Action Changed;
		public event Action<CombatRelationClickEvent> Click;
		public event Action<CombatRelationClickEvent> Give;

		public int Id
		{
			get { return id; }
		}

		public int Balance
		{
			get { return balance; }
			set
			{
				balance = value;
				UpdateLabel();
			}
		}

		public int Intensity
		{
			get { return intensity; }
			set
			{
				intensity = value;
				UpdateLabel();
				Changed.Raise();
			}
		}

		public int Initiative
		{
			get { return initiative; }
			set
			{
				initiative = value;
				Changed.Raise();
			}
		}
		
		public int EnemyInitiative { get; set; }
		
		public int Offense { get; set; }
		
		public int Defense { get; set; }

		public int GiveState
		{
			get { return button.State; }
			set
			{
				button.State = value;
				Changed.Raise();
			}
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(background, 0, 0);
		}

		private void UpdateLabel()
		{
			label.Text = string.Format("{0} {1}", Balance, Intensity);
		}

		private void OnAvatarClick(AvatarView sender, MouseButtonEvent e)
		{
			Click.Raise(new CombatRelationClickEvent(e.Button, this));
		}

		private void OnButtonClick(MouseButtonEvent e)
		{
			Give.Raise(new CombatRelationClickEvent(e.Button, this));
		}
	}
}
