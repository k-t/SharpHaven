﻿using System;
using System.Collections.Generic;
using System.Linq;
using Haven;
using SharpHaven.Client;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Text;
using SharpHaven.UI.Layouts;
using SharpHaven.Utils;

namespace SharpHaven.UI.Widgets
{
	public class CharWindow : Window
	{
		public static readonly Color DebuffColor = Color.FromArgb(255, 128, 128);
		public static readonly Color BuffColor = Color.FromArgb(128, 255, 128);

		private static readonly Drawable MinusUp;
		private static readonly Drawable MinusDown;
		private static readonly Drawable PlusUp;
		private static readonly Drawable PlusDown;

		static CharWindow()
		{
			MinusUp = App.Resources.Get<Drawable>("gfx/hud/charsh/minusup");
			MinusDown = App.Resources.Get<Drawable>("gfx/hud/charsh/minusdown");
			PlusUp = App.Resources.Get<Drawable>("gfx/hud/charsh/plusup");
			PlusDown = App.Resources.Get<Drawable>("gfx/hud/charsh/plusdown");
		}

		private readonly ClientSession session;
		private int exp;
		private List<AttributeBinding> attrBindings;

		private readonly List<Container> tabs;
		private Container tabAttr;
		private Container tabStudy;
		private Container tabSkills;
		private Container tabBeliefs;
		private Label lblExpValue;
		private Label lblExpModValue;
		private Label lblCostValue;
		private Label lblAttentionValue;
		private FoodMeter foodMeter;
		private BeliefTimer beliefTimer;
		private readonly List<BeliefWidget> beliefWidgets;
		private SkillList lstAvailableSkills;
		private SkillList lstCurrentSkills;
		private Label lblSkillCost;
		private Worship worship;

		private readonly GridLayout baseLayout;
		private readonly GridLayout skillLayout;
		private readonly GridLayout beliefLayout;

		public CharWindow(Widget parent, ClientSession session) : base(parent, "Character Sheet")
		{
			this.Resize(400, 340);

			this.session = session;
			this.tabs = new List<Container>();
			this.attrBindings = new List<AttributeBinding>();
			this.beliefWidgets = new List<BeliefWidget>();

			skillLayout = new GridLayout();
			skillLayout.SetColumnWidth(0, 20);
			skillLayout.SetColumnWidth(1, 100);
			skillLayout.SetColumnWidth(2, 30);
			skillLayout.SetColumnWidth(3, 15);

			baseLayout = new GridLayout();
			baseLayout.SetColumnWidth(0, 20);
			baseLayout.SetColumnWidth(1, 75);

			beliefLayout = new GridLayout();

			InitAttributesTab();
			InitSkillsTab();
			InitBeliefsTab();
			InitStudyTab();

			int bx = 10;
			AddTabButton(bx, "Attributes", "gfx/hud/charsh/attribup", "gfx/hud/charsh/attribdown", tabAttr);
			AddTabButton(bx += 70, "Study", "gfx/hud/charsh/ideasup", "gfx/hud/charsh/ideasdown", tabStudy);
			AddTabButton(bx += 70, "Skills", "gfx/hud/charsh/skillsup", "gfx/hud/charsh/skillsdown", tabSkills);
			AddTabButton(bx += 70, "Personal Beliefs", "gfx/hud/charsh/worshipup", "gfx/hud/charsh/worshipdown", tabBeliefs);

			SetTab(tabAttr);
			Pack();
		}

		public event Action<object[]> AttributesChanged;
		public event Action<BeliefChangeEvent> BeliefChanged;
		public event Action<Skill> SkillLearned;

		public Container Study
		{
			get { return tabStudy; }
		}

		public BeliefTimer BeliefTimer
		{
			get { return beliefTimer; }
		}

		public FoodMeter FoodMeter
		{
			get { return foodMeter; }
		}

		public SkillList AvailableSkills
		{
			get { return lstAvailableSkills; }
		}

		public SkillList CurrentSkills
		{
			get { return lstCurrentSkills; }
		}

		public Worship Worship
		{
			get { return worship; }
		}

		public void SetAttention(int value)
		{
			lblAttentionValue.Text = value.ToString();
		}

		public void SetExp(int value)
		{
			exp = value;
			lblExpValue.Text = value.ToString();
			lstAvailableSkills.MaxCost = exp;
		}

		protected override void OnDispose()
		{
			base.OnDispose();
			foreach (var binding in attrBindings)
				binding.Dispose();
		}

		private void AddTabButton(int x, string tooltip, string image, string pressedImage, Container tab)
		{
			var button = new ImageButton(this);
			button.Tooltip = new Tooltip(tooltip);
			button.Move(x, 310);
			button.Image = App.Resources.Get<Drawable>(image);
			button.PressedImage = App.Resources.Get<Drawable>(pressedImage);
			button.Resize(button.Image.Size);
			button.Click += () => SetTab(tab);
		}

		private void SetTab(Container tab)
		{
			foreach (var t in tabs)
				t.Visible = (t == tab);
		}

		#region Attributes

		private void InitAttributesTab()
		{
			tabAttr = new Container(this);
			tabAttr.Resize(400, 300);
			tabs.Add(tabAttr);

			// LP
			var lblCost = new Label(tabAttr, Fonts.LabelText);
			lblCost.Text = "Cost:";
			lblCostValue = new Label(tabAttr, Fonts.LabelText);
			lblCostValue.Text = "0";

			var lblExp = new Label(tabAttr, Fonts.LabelText);
			lblExp.Text = "Learning Points:";
			lblExpValue = new Label(tabAttr, Fonts.LabelText);
			lblExpValue.Text = "0";

			var lblExpMod = new Label(tabAttr, Fonts.LabelText);
			lblExpMod.Text = "Learning Ability:";
			lblExpModValue = new Label(tabAttr, Fonts.LabelText);

			var expMod = session.Attributes["expmod"];
			attrBindings.Add(new ExpModBinding(expMod, lblExpModValue));

			var btnBuy = new Button(tabAttr, 75);
			btnBuy.Text = "Buy";
			btnBuy.Click += BuySkills;

			var lpLayout = new GridLayout();
			lpLayout.SetColumnWidth(0, 90);
			lpLayout.AddWidget(lblCost, 0, 0);
			lpLayout.AddWidget(lblCostValue, 0, 1);
			lpLayout.AddWidget(lblExp, 1, 0);
			lpLayout.AddWidget(lblExpValue, 1, 1);
			lpLayout.AddWidget(lblExpMod, 2, 0);
			lpLayout.AddWidget(lblExpModValue, 2, 1);
			lpLayout.AddWidget(btnBuy, 3, 0);
			lpLayout.UpdateGeometry(210, 220, 0, 0);

			// Base Attributes
			var lblBase = new Label(tabAttr, Fonts.LabelText);
			lblBase.Move(10, 10);
			lblBase.Text = "Base Attributes:";
			AddBase("str", "Strength");
			AddBase("agil", "Agility");
			AddBase("intel", "Intelligence");
			AddBase("cons", "Constitution");
			AddBase("perc", "Perception");
			AddBase("csm", "Charisma");
			AddBase("dxt", "Dexterity");
			AddBase("psy", "Psyche");
			baseLayout.UpdateGeometry(10, 40, 0, 0);

			// Skills
			var lblSkill = new Label(tabAttr, Fonts.LabelText);
			lblSkill.Move(210, 10);
			lblSkill.Text = "Skill Values:";
			AddSkill("unarmed", "Unarmed Combat");
			AddSkill("melee", "Melee Combat");
			AddSkill("ranged", "Marksmanship");
			AddSkill("explore", "Exploration");
			AddSkill("stealth", "Stealth");
			AddSkill("sewing", "Sewing");
			AddSkill("smithing", "Smithing");
			AddSkill("carpentry", "Carpentry");
			AddSkill("cooking", "Cooking");
			AddSkill("farming", "Farming");
			AddSkill("survive", "Survival");
			skillLayout.UpdateGeometry(210, 40, 0, 0);

			// Food
			foodMeter = new FoodMeter(tabAttr);
			foodMeter.Move(10, 180);
		}

		private void AddBase(string name, string title)
		{
			var image = new Image(tabAttr);
			image.Drawable = App.Resources.Get<Drawable>("gfx/hud/charsh/" + name);

			var lblName = new Label(tabAttr, Fonts.LabelText);
			lblName.Text = title + ":";

			var lblValue = new Label(tabAttr, Fonts.LabelText);
			lblValue.AutoSize = true;

			int row = baseLayout.RowCount;
			baseLayout.AddWidget(image, row, 0);
			baseLayout.AddWidget(lblName, row, 1);
			baseLayout.AddWidget(lblValue, row, 2);

			var attr = session.Attributes[name];
			attrBindings.Add(new BaseAttributeBinding(attr, lblValue));
		}

		private void AddSkill(string name, string title)
		{
			var attr = session.Attributes[name];

			var image = new Image(tabAttr);
			image.Drawable = App.Resources.Get<Drawable>("gfx/hud/charsh/" + name);

			var lblName = new Label(tabAttr, Fonts.LabelText);
			lblName.Text = title + ":";

			var lblValue = new Label(tabAttr, Fonts.LabelText);
			lblValue.AutoSize = true;
			var skill = new SkillAttributeBinding(attr, lblValue);
			skill.CostChanged += UpdateCost;
			attrBindings.Add(skill);

			var btnMinus = new ImageButton(tabAttr);
			btnMinus.Image = MinusUp;
			btnMinus.PressedImage = MinusDown;
			btnMinus.Click += () => skill.Add(-1);
			btnMinus.MouseWheel += arg => skill.Add(Math.Sign(arg.Delta));

			var btnPlus = new ImageButton(tabAttr);
			btnPlus.Image = PlusUp;
			btnPlus.PressedImage = PlusDown;
			btnPlus.Click += () => skill.Add(1);
			btnPlus.MouseWheel += arg => skill.Add(Math.Sign(arg.Delta));

			int row = skillLayout.RowCount;
			skillLayout.AddWidget(image, row, 0);
			skillLayout.AddWidget(lblName, row, 1);
			skillLayout.AddWidget(lblValue, row, 2);
			skillLayout.AddWidget(btnMinus, row, 3);
			skillLayout.AddWidget(btnPlus, row, 4);
		}

		private void UpdateCost()
		{
			int cost = 0;
			foreach (var skill in attrBindings.OfType<SkillAttributeBinding>())
				cost += skill.Cost;

			lblCostValue.Text = cost.ToString();
			lblCostValue.TextColor = cost > exp ? Color.FromArgb(255, 128, 128) : Color.White;
		}

		private void BuySkills()
		{
			var args = new List<object>();
			foreach (var skill in attrBindings.OfType<SkillAttributeBinding>())
			{
				args.Add(skill.AttributeName);
				args.Add(skill.BaseValue);
			}
			AttributesChanged.Raise(args.ToArray());
		}

		#endregion

		#region Study

		private void InitStudyTab()
		{
			tabStudy = new Container(this);
			tabStudy.Resize(400, 275);
			tabs.Add(tabStudy);

			var lblAttention = new Label(tabStudy, Fonts.LabelText);
			lblAttention.Text = "Used attention:";
			lblAttention.Move(138, 210);

			lblAttentionValue = new Label(tabStudy, Fonts.LabelText);
			lblAttentionValue.Move(240, 210);

			var lblLimit = new Label(tabStudy, Fonts.LabelText);
			lblLimit.Text = "Attention limit:";
			lblLimit.Move(138, 225);

			var lblLimitValue = new Label(tabStudy, Fonts.LabelText);
			lblLimitValue.Move(240, 225);

			var intelAttr = session.Attributes["intel"];
			attrBindings.Add(new BaseAttributeBinding(intelAttr, lblLimitValue));
		}

		#endregion

		#region Skills

		private void InitSkillsTab()
		{
			tabSkills = new Container(this);
			tabSkills.Resize(400, 275);
			tabs.Add(tabSkills);

			lblSkillCost = new Label(tabSkills, Fonts.LabelText);
			lblSkillCost.Move(300, 130);
			lblSkillCost.Text = "Cost: N/A";

			var skillInfo = new SkillInfo(tabSkills);
			skillInfo.Move(10, 10);
			skillInfo.Resize(180, 260);

			var lblAvailableSkills = new Label(tabSkills, Fonts.LabelText);
			lblAvailableSkills.Move(210, 10);
			lblAvailableSkills.Text = "Available Skills:";

			lstAvailableSkills = new SkillList(tabSkills);
			lstAvailableSkills.Move(210, 25);
			lstAvailableSkills.Resize(180, 100);
			lstAvailableSkills.SelectedItemChanged += OnSelectedAvailableSkillChange;

			var lblCurrentSkills = new Label(tabSkills, Fonts.LabelText);
			lblCurrentSkills.Move(210, 155);
			lblCurrentSkills.Text = "Current Skills:";

			lstCurrentSkills = new SkillList(tabSkills);
			lstCurrentSkills.Move(210, 170);
			lstCurrentSkills.Resize(180, 100);
			lstCurrentSkills.SelectedItemChanged += OnSelectedCurrentSkillChange;

			var btnLearn = new Button(tabSkills, 75);
			btnLearn.Move(210, 130);
			btnLearn.Text = "Learn";
			btnLearn.Click += LearnSkill;
		}

		private void LearnSkill()
		{
			if (lstAvailableSkills.SelectedItem != null)
				SkillLearned.Raise(lstAvailableSkills.SelectedItem);
		}

		private void OnSelectedAvailableSkillChange()
		{
			if (lstAvailableSkills.SelectedItem != null)
			{
				lstCurrentSkills.SelectedItem = null;
				lblSkillCost.Text = $"Cost: {lstAvailableSkills.SelectedItem.Cost}";
			}
			else
				lblSkillCost.Text = "Cost: N/A";
		}

		private void OnSelectedCurrentSkillChange()
		{
			if (lstCurrentSkills.SelectedItem != null)
				lstAvailableSkills.SelectedItem = null;
		}

		#endregion

		#region Beliefs

		private void InitBeliefsTab()
		{
			tabBeliefs = new Container(this);
			tabBeliefs.Resize(400, 275);
			tabs.Add(tabBeliefs);

			beliefTimer = new BeliefTimer(tabBeliefs);
			beliefTimer.Move(10, 10);
			beliefTimer.TimeChanged += HandleBeliefTimeChange;

			AddBelief("life", "death", "life", false);
			AddBelief("night", "night", "day", true);
			AddBelief("civil", "barbarism", "civilization", false);
			AddBelief("nature", "nature", "industry", true);
			AddBelief("martial", "martial", "peaceful", true);
			AddBelief("change", "tradition", "change", false);

			beliefLayout.UpdateGeometry(18, 50, 0, 0);

			worship = new Worship(tabBeliefs);
			worship.Title = "The Ancestors";
			worship.Move(255, 40);
		}

		private void AddBelief(string name, string left, string right, bool inv)
		{
			var widget = new BeliefWidget(tabBeliefs, left, right);
			widget.SliderMoved += (delta) => HandleBeliefChange(name, delta, inv);
			beliefWidgets.Add(widget);

			var label = new Label(tabBeliefs, Fonts.LabelText);
			label.TextAlign = TextAlign.Center;
			label.Text = string.Format("{0} / {1}", left.ToTitleCase(), right.ToTitleCase());
			label.Resize(widget.Width, label.Height);

			var attr = session.Attributes[name];
			attrBindings.Add(new BeliefBinding(attr, widget, inv));

			int row = beliefLayout.RowCount;
			beliefLayout.AddWidget(label, row, 0);
			beliefLayout.AddWidget(widget, row + 1, 0);
		}

		private void HandleBeliefTimeChange()
		{
			foreach (var widget in beliefWidgets)
				widget.Enabled = !(beliefTimer.Time > 0);
		}

		private void HandleBeliefChange(string name, int delta, bool inv)
		{
			BeliefChanged.Raise(new BeliefChangeEvent(name, delta, inv));
		}

		#endregion
	}
}
