using System;
using System.Collections.Generic;
using SharpHaven.Graphics;

namespace SharpHaven.UI.Widgets
{
	public class ClaimWindow : Window
	{
		private readonly Label lblArea;
		private readonly Label lblCost;
		private readonly GroupSelector selector;
		private List<RightCheckBox> chkRights;

		private Rect area;
		private Rect selectedArea;
		private readonly ClaimRight[] rights;

		public ClaimWindow(Widget parent, Rect area)
			: base(parent, "Stake")
		{
			this.area = area;
			this.selectedArea = area;
			this.rights = new ClaimRight[BuddyGroup.Colors.Length];

			lblArea = new Label(this, Fonts.LabelText);

			lblCost = new Label(this, Fonts.LabelText);
			lblCost.Move(0, 15);

			var btnNorth = new Button(this, 80);
			btnNorth.Move(60, 40);
			btnNorth.Text = "Extend North";
			btnNorth.Click += () => Extend(1, 0, 0, 0);

			var btnEast = new Button(this, 80);
			btnEast.Move(120, 65);
			btnEast.Text = "Extend East";
			btnEast.Click += () => Extend(0, 1, 0, 0);

			var btnSouth = new Button(this, 80);
			btnSouth.Move(60, 90);
			btnSouth.Text = "Extend South";
			btnSouth.Click += () => Extend(0, 0, 1, 0);

			var btnWest = new Button(this, 80);
			btnWest.Move(0, 65);
			btnWest.Text = "Extend West";
			btnWest.Click += () => Extend(0, 0, 0, 1);

			var btnBuy = new Button(this, 60);
			btnBuy.Move(0, 190);
			btnBuy.Text = "Buy";
			btnBuy.Click += () => Buy.Raise(this, EventArgs.Empty);

			var btnReset = new Button(this, 60);
			btnReset.Move(80, 190);
			btnReset.Text = "Reset";
			btnReset.Click += Reset;

			var btnDestroy = new Button(this, 60);
			btnDestroy.Move(160, 190);
			btnDestroy.Text = "Declaim";
			btnDestroy.Click += () => Declaim.Raise(this, EventArgs.Empty);

			var lblAssign = new Label(this, Fonts.LabelText);
			lblAssign.Move(0, 120);
			lblAssign.Text = "Assign permissions to memorized people:";

			selector = new GroupSelector(this);
			selector.Group = 0;
			selector.Move(0, 135);
			selector.Select += OnGroupSelect;

			var chkTrespass = new RightCheckBox(this, ClaimRight.Trespassing);
			chkTrespass.Move(0, 155);
			chkTrespass.CheckedChanged += ToggleRight;

			var chkTheft = new RightCheckBox(this, ClaimRight.Theft);
			chkTheft.Move(90, 155);
			chkTheft.CheckedChanged += ToggleRight;

			var chkVandalism = new RightCheckBox(this, ClaimRight.Vandalism);
			chkVandalism.Move(145, 155);
			chkVandalism.CheckedChanged += ToggleRight;

			chkRights = new List<RightCheckBox>();
			chkRights.AddRange(new [] { chkTrespass, chkTheft, chkVandalism });

			UpdateLabels();
			Pack();
		}

		public event EventHandler SelectedAreaChanged;
		public event EventHandler<ClaimRightsChangeEvent> RightsChanged;
		public event EventHandler Buy;
		public event EventHandler Declaim;

		public Rect Area
		{
			get { return area; }
			set
			{
				area = value;
				SelectedArea = value;
			}
		}

		public Rect SelectedArea
		{
			get { return selectedArea; }
			set
			{
				selectedArea = value;
				UpdateLabels();
				SelectedAreaChanged.Raise(this, EventArgs.Empty);
			}
		}

		public void SetRight(int group, ClaimRight right)
		{
			rights[group] = right;
			if (selector.Group == group)
				UpdateRights();
		}

		private void Extend(int n, int e, int s, int w)
		{
			SelectedArea = new Rect(
				SelectedArea.X - w,
				SelectedArea.Y - n,
				SelectedArea.Width + e + w,
				SelectedArea.Height + s + n);
		}

		private void Reset()
		{
			SelectedArea = area;
		}

		private void ToggleRight(object sender, EventArgs e)
		{
			if (!selector.Group.HasValue)
				return;

			var checkBox = (RightCheckBox)sender;
			var right = checkBox.Right;

			var changed = false;
			var groupRights = rights[selector.Group.Value];
			if (groupRights.HasFlag(right) && !checkBox.IsChecked)
			{
				groupRights &= ~right;
				changed = true;
			}
			else if (!groupRights.HasFlag(right) && checkBox.IsChecked)
			{
				groupRights |= right;
				changed = true;
			}

			if (changed)
			{
				rights[selector.Group.Value] = groupRights;
				RightsChanged.Raise(this,
					new ClaimRightsChangeEvent(selector.Group.Value, groupRights));
			}
		}

		private void UpdateRights()
		{
			var groupRights = selector.Group.HasValue
				? rights[selector.Group.Value]
				: 0;

			foreach (var checkBox in chkRights)
				checkBox.IsChecked = groupRights.HasFlag(checkBox.Right);
		}

		private void UpdateLabels()
		{
			int oldSize = area.Width * area.Height;
			int newSize = selectedArea.Width * selectedArea.Height;

			lblArea.Text = $"Area: {newSize} m²";
			lblCost.Text = $"Cost: {(newSize - oldSize) * 10}";
		}

		protected void OnGroupSelect(int group)
		{
			selector.Group = group;
			UpdateRights();
		}

		private class RightCheckBox : CheckBox
		{
			public RightCheckBox(Widget parent, ClaimRight right)
				: base(parent)
			{
				Right = right;
				Text = right.ToString();
			}

			public ClaimRight Right { get; }
		}
	}
}
