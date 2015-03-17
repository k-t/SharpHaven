using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.UI.Layouts;

namespace MonoHaven.UI.Widgets
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
			MinusUp = App.Resources.GetImage("gfx/hud/charsh/minusup");
			MinusDown = App.Resources.GetImage("gfx/hud/charsh/minusdown");
			PlusUp = App.Resources.GetImage("gfx/hud/charsh/plusup");
			PlusDown = App.Resources.GetImage("gfx/hud/charsh/plusdown");
		}

		private readonly GameState gstate;
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
		private FoodMeter foodMeter;

		private readonly GridLayout baseLayout;
		private readonly GridLayout skillLayout;

		public CharWindow(Widget parent, GameState gstate) : base(parent, "Character Sheet")
		{
			Resize(400, 340);

			this.gstate = gstate;
			this.tabs = new List<Container>();
			this.attrBindings = new List<AttributeBinding>();
			
			skillLayout = new GridLayout();
			skillLayout.SetColumnWidth(0, 20);
			skillLayout.SetColumnWidth(1, 90);
			skillLayout.SetColumnWidth(2, 30);
			skillLayout.SetColumnWidth(3, 15);

			baseLayout = new GridLayout();
			baseLayout.SetColumnWidth(0, 20);
			baseLayout.SetColumnWidth(1, 70);

			InitAttributesTab();
			InitSkillsTab();
			InitBeliefsTab();
			InitStudyTab();

			int bx = 10;
			AddTabButton(bx, "gfx/hud/charsh/attribup", "gfx/hud/charsh/attribdown", tabAttr);
			AddTabButton(bx += 70, "gfx/hud/charsh/ideasup", "gfx/hud/charsh/ideasdown", tabStudy);
			AddTabButton(bx += 70, "gfx/hud/charsh/skillsup", "gfx/hud/charsh/skillsdown", tabSkills);
			AddTabButton(bx += 70, "gfx/hud/charsh/worshipup", "gfx/hud/charsh/worshipdown", tabBeliefs);

			SetTab(tabAttr);
			Pack();
		}

		public event Action<object[]> AttributesBought;

		public Widget Study
		{
			get { return tabStudy; }
		}

		public FoodMeter FoodMeter
		{
			get { return foodMeter; }
		}

		public void SetExp(int value)
		{
			exp = value;
			lblExpValue.Text = value.ToString();
		}

		protected override void OnDispose()
		{
			base.OnDispose();
			foreach (var binding in attrBindings)
				binding.Dispose();
		}

		private void AddTabButton(int x, string image, string pressedImage, Container tab)
		{
			var button = new ImageButton(this);
			button.Move(x, 310);
			button.Image = App.Resources.GetImage(image);
			button.PressedImage = App.Resources.GetImage(pressedImage);
			button.Resize(button.Image.Size);
			button.Clicked += () => SetTab(tab);
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

			var expMod = gstate.GetAttr("expmod");
			attrBindings.Add(new ExpModBinding(expMod, lblExpModValue));

			var btnBuy = new Button(tabAttr, 75);
			btnBuy.Text = "Buy";
			btnBuy.Clicked += BuySkills;

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
			image.Drawable = App.Resources.GetImage("gfx/hud/charsh/" + name);

			var lblName = new Label(tabAttr, Fonts.LabelText);
			lblName.Text = title + ":";

			var lblValue = new Label(tabAttr, Fonts.LabelText);

			int row = baseLayout.RowCount;
			baseLayout.AddWidget(image, row, 0);
			baseLayout.AddWidget(lblName, row, 1);
			baseLayout.AddWidget(lblValue, row, 2);

			var attr = gstate.GetAttr(name);
			attrBindings.Add(new BaseAttributeBinding(attr, lblValue));
		}

		private void AddSkill(string name, string title)
		{
			var attr = gstate.GetAttr(name);

			var image = new Image(tabAttr);
			image.Drawable = App.Resources.GetImage("gfx/hud/charsh/" + name);
			
			var lblName = new Label(tabAttr, Fonts.LabelText);
			lblName.Text = title + ":";
			
			var lblValue = new Label(tabAttr, Fonts.LabelText);
			var skill = new SkillAttributeBinding(attr, lblValue);
			skill.CostChanged += UpdateCost;
			attrBindings.Add(skill);

			var btnMinus = new ImageButton(tabAttr);
			btnMinus.Image = MinusUp;
			btnMinus.PressedImage = MinusDown;
			btnMinus.Clicked += () => skill.Add(-1);
			btnMinus.MouseWheel += arg => skill.Add(Math.Sign(arg.Delta));

			var btnPlus = new ImageButton(tabAttr);
			btnPlus.Image = PlusUp;
			btnPlus.PressedImage = PlusDown;
			btnPlus.Clicked += () => skill.Add(1);
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
			AttributesBought.Raise(args.ToArray());
		}

		#endregion

		#region Study

		private void InitStudyTab()
		{
			tabStudy = new Container(this);
			tabStudy.Resize(400, 275);
			tabs.Add(tabStudy);
		}

		#endregion

		#region Skills

		private void InitSkillsTab()
		{
			tabSkills = new Container(this);
			tabSkills.Resize(400, 275);
			tabs.Add(tabSkills);
		}

		#endregion

		#region Beliefs

		private void InitBeliefsTab()
		{
			tabBeliefs = new Container(this);
			tabBeliefs.Resize(400, 275);
			tabs.Add(tabBeliefs);
		}

		#endregion
	}
}
